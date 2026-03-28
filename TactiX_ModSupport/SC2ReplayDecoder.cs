using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using NLog;
using s2protocol.NET;
using s2protocol.NET.Models;
using TactiX_Localization;
using TactiX_Logger;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;

namespace TactiX_ModSupport;

public class Sc2ReplayDecoder : IReplayDecoder
{
    private const string DATA_DICT_JSON = "SC2ProductionDuration.json";

    private readonly Logger _logger;
    private readonly IMessenger _messenger;
    private readonly ILocalizationService _localizationService;

    private readonly ReplayDecoderOptions _options;

    private readonly Sc2DataDict _sc2Data;

    private Dictionary<int, string> _playerNames;

    /// <summary>
    ///     建筑的升级变形事件是不包括playerId的，因此尝试用种族分配来处理
    ///     如果种族不重复，则按照种族来分配升级的建筑，否则跳过解析
    /// </summary>
    private Dictionary<string, string> _raceDict;

    private Dictionary<string, List<LReplayAction>> _replayActionDict;
    private Dictionary<string, int> _supplyCostDict;

    /// <summary>
    ///     待处理的人口变化事件集合
    /// </summary>
    private Dictionary<string, List<DoneEvtRecord>> _supplySupportChangeDict;

    private Dictionary<string, int> _supplySupportDict;


#pragma warning disable CS8618
    public Sc2ReplayDecoder(ILoggerContainer loggerContainer, IMessenger messenger,
        ILocalizationService localizationService)
#pragma warning restore CS8618
    {
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _messenger = messenger;
        _localizationService = localizationService;

        // 为了避免UI预览器崩溃因此加了过滤
        // 实际逻辑没有这么复杂
        if (File.Exists(DATA_DICT_JSON))
        {
            var txt = File.ReadAllText(DATA_DICT_JSON);
            if (string.IsNullOrEmpty(txt)) throw new Exception("CANT FIND SC2ProductionDuration.json");
            _sc2Data = JsonConvert.DeserializeObject<Sc2DataDict>(txt)!;
            if (_sc2Data == null) throw new Exception("SC2ProductionDuration.json File is Broken");
        }

        // 初始化字典，后续使用 Clear() 复用以减少 GC 压力
        _playerNames = new();
        _supplyCostDict = new();
        _supplySupportDict = new();
        _supplySupportChangeDict = new();
        _raceDict = new();
        _replayActionDict = new();

        _options = new ReplayDecoderOptions
        {
            Details = true,
            Metadata = true,
            MessageEvents = true,
            TrackerEvents = true,
            GameEvents = true,
            AttributeEvents = true
        };
    }

    private Dictionary<string, int> UnitData => _sc2Data.Units;
    private Dictionary<string, int> TechData => _sc2Data.Techs;
    private Dictionary<string, string> UnitsDict => _sc2Data.UnitsDict;
    private Dictionary<string, int> SupplyCost => _sc2Data.SupplyCost;
    private Dictionary<string, int> SupplySupport => _sc2Data.SupplySupport;
    private List<string> Ignore => _sc2Data.Ignore;
    private List<string> TerranBuildingTypeChange => _sc2Data.TerranBuildingTypeChange;
    private List<string> ZergBuildingTypeChange => _sc2Data.ZergBuildingTypeChange;

    public async Task<Dictionary<string, List<LReplayAction>>> DecodeReplay(string replayPath)
    {
        try
        {
            _playerNames.Clear();
            _supplyCostDict.Clear();
            _supplySupportDict.Clear();
            _supplySupportChangeDict.Clear();
            _raceDict.Clear();
            _replayActionDict.Clear();

            ReplayDecoder decoder = new();
            var replay = await decoder.DecodeAsync(replayPath, _options);

            if (replay == null
                || replay.Details == null
                || replay.TrackerEvents == null) return [];

            var sameRace = HasDuplicateRace(replay);

            var playerIndex = 1;
            foreach (var player in replay.Details.Players)
            {
                var playerName = $"{player.Name}_{playerIndex}"; // 规避出现同样ID（主要是人机）的情况
                _playerNames.Add(playerIndex, playerName);

                _replayActionDict.EnsureKeyExists(playerName);
                _supplyCostDict.EnsureKeyExists(playerName);
                _supplySupportDict.EnsureKeyExists(playerName);
                _supplySupportChangeDict.EnsureKeyExists(playerName);

                _supplyCostDict[playerName] = 12;
                _supplySupportDict[playerName] = player.Race switch // 不同语言客户端的rep种族字段也会不一样
                {
                    "Protoss" => 15,
                    "Terran" => 15,
                    "Zerg" => 14,
                    "星灵" => 15,
                    "人类" => 15,
                    "异虫" => 14,
                    _ => 15
                };

                if (!sameRace) _raceDict.Add(player.Race, playerName);

                playerIndex++;
            }

            var trackers = new List<TrackerEvent>(
                replay.TrackerEvents.SUnitInitEvents.Count +
                replay.TrackerEvents.SUnitBornEvents.Count +
                replay.TrackerEvents.SUpgradeEvents.Count +
                replay.TrackerEvents.SUnitTypeChangeEvents.Count +
                replay.TrackerEvents.SUnitDoneEvents.Count);
            trackers.AddRange(replay.TrackerEvents.SUnitInitEvents);
            trackers.AddRange(replay.TrackerEvents.SUnitBornEvents);
            trackers.AddRange(replay.TrackerEvents.SUpgradeEvents);
            trackers.AddRange(replay.TrackerEvents.SUnitTypeChangeEvents);
            trackers.AddRange(replay.TrackerEvents.SUnitDoneEvents);
            trackers.Sort(static (a, b) => a.Gameloop.CompareTo(b.Gameloop));

            for (var i = 0; i < trackers.Count; i++)
            {
                var evt = trackers[i];
                var gameloop = evt.Gameloop;

                if (evt.Gameloop == 0) continue;

                switch (evt.EventType)
                {
                    case TrackerEventType.SUnitInitEvent:
                        HandleSUnitInitEvent((SUnitInitEvent)evt);
                        break;
                    case TrackerEventType.SUnitBornEvent:
                        HandleSUnitBornEvent((SUnitBornEvent)evt);
                        break;
                    case TrackerEventType.SUpgradeEvent:
                        HandleSUpgradeEvent((SUpgradeEvent)evt);
                        break;
                    case TrackerEventType.SUnitTypeChangeEvent:
                        HandleSUnitTypeChangeEvent((SUnitTypeChangeEvent)evt, sameRace);
                        break;
                    case TrackerEventType.SUnitDoneEvent:
                        HandleSUnitDoneEvent((SUnitDoneEvent)evt);
                        break;
                }
            }

            foreach (var playerName in _replayActionDict.Keys)
            {
                _replayActionDict[playerName].Sort(static (a, b) => a.Gameloop.CompareTo(b.Gameloop));

                AdjustTime(_replayActionDict[playerName]);
            }

            return _replayActionDict;
        }
        catch (Exception ex)
        {
            _logger.Error($"DecodeReplay Error: {ex.Message}");
            _messenger.Send(new MbToastPureText()
            {
                Message = string.Format(
                    _localizationService.GetString("ReplayAnalysisDecodeError"),
                    ex.Message),
                Title = _localizationService.GetString("ReplayAnalysisDecodeErrorTitle"),
                Type = MbEnumToastType.Error
            });
            return [];
        }
    }

    private void HandleSUnitInitEvent(SUnitInitEvent evt)
    {
        if (evt.ControlPlayerId == 0) // 排除非玩家控制单位
        {
            _logger.Warn(evt.UnitTypeName);
            return;
        }

        var unitName = evt.UnitTypeName;
        if (Ignore.Contains(unitName)) return; // 过滤单位

        if (!UnitsDict.TryGetValue(unitName, out var unitAbbr))
        {
            _logger.Error($"Error Unit Name: {unitName}");
            return;
        }

        var playerName = _playerNames[evt.ControlPlayerId];

        if (SupplyCost.TryGetValue(unitName, out var cost)) // 单位生产消耗的人口是立即的
            _supplyCostDict[playerName] += cost;

        if (SupplySupport.TryGetValue(unitName, out var support) && evt.SUnitDoneEvent != null) // 人口增加的单位/建筑是完成后才生效的
            _supplySupportChangeDict[playerName]
                .Add(new DoneEvtRecord(evt.SUnitDoneEvent.Gameloop, support));

        _replayActionDict[playerName].Add(new LReplayAction
        {
            UnitName = unitName,
            Gameloop = evt.Gameloop,
            Time = (int)Math.Floor(evt.Gameloop / 22.4),
            Abbr = unitAbbr,
            Supply = $"{_supplyCostDict[playerName]}/{_supplySupportDict[playerName]}"
        });
    }

    private void HandleSUnitBornEvent(SUnitBornEvent evt)
    {
        try
        {
            if (evt.ControlPlayerId == 0) // 排除非玩家控制单位
            {
                _logger.Warn(evt.UnitTypeName);
                return;
            }

            var unitName = evt.UnitTypeName;
            if (Ignore.Contains(unitName)) return; // 过滤单位

            if (!UnitData.TryGetValue(unitName, out var unitTime))
            {
                _logger.Error($"Error Unit Name: {unitName}");
                return;
            }

            var playerName = _playerNames[evt.ControlPlayerId];
            var startLoop = evt.Gameloop - unitTime * 16;

            if (SupplyCost.TryGetValue(unitName, out var cost)) // 单位生产消耗的人口是立即的
                _supplyCostDict[playerName] += cost;

            if (SupplySupport.TryGetValue(unitName, out var supply)) // 人口增加的单位/建筑是完成后才生效的；
                // Born为已完成，直接增加
                _supplySupportDict[playerName] += supply;

            _replayActionDict[playerName].Add(new LReplayAction
            {
                UnitName = unitName,
                Gameloop = startLoop,
                Time = (int)Math.Floor(startLoop / 22.4),
                Abbr = UnitsDict[unitName],
                Supply = $"{_supplyCostDict[playerName]}/{_supplySupportDict[playerName]}"
            });
        }
        catch (Exception e)
        {
            _logger.Error($"HandleSUnitBornEvent Error: {e.Message}");
        }
    }

    private void HandleSUpgradeEvent(SUpgradeEvent evt)
    {
        var upgradeName = evt.UpgradeTypeName;
        if (Ignore.Contains(upgradeName)) return; // 过滤升级

        var playerName = _playerNames[evt.PlayerId];

        if (!TechData.TryGetValue(upgradeName, out var techTime))
        {
            _logger.Error($"Error Upgrade Name: {upgradeName}");
            return;
        }

        var startLoop = evt.Gameloop - techTime * 16;

        _replayActionDict[playerName].Add(new LReplayAction
        {
            UnitName = upgradeName,
            Gameloop = startLoop,
            Time = (int)Math.Floor(startLoop / 22.4),
            Abbr = UnitsDict[upgradeName],
            Supply = $"{_supplyCostDict[playerName]}/{_supplySupportDict[playerName]}"
        });
    }

    private void HandleSUnitTypeChangeEvent(SUnitTypeChangeEvent evt, bool isSameRace)
    {
        if (isSameRace) return;

        var unitName = evt.UnitTypeName;
        if (Ignore.Contains(unitName)) return;
        if (!UnitsDict.TryGetValue(unitName, out var unitAbbr))
        {
            _logger.Error($"Error Unit Name: {unitName}");
            return;
        }

        string? playerName = null;

        var isTerran = TerranBuildingTypeChange.Contains(unitName);
        var isZerg = ZergBuildingTypeChange.Contains(unitName);

        if (isTerran)
            playerName = _raceDict.TryGetValue("Terran", out var terranName)
                ? terranName
                : _raceDict.TryGetValue("人类", out var chineseName)
                    ? chineseName
                    : null;

        if (isZerg)
            playerName = _raceDict.TryGetValue("Zerg", out var terranName)
                ? terranName
                : _raceDict.TryGetValue("异虫", out var chineseName)
                    ? chineseName
                    : null;

        if (string.IsNullOrEmpty(playerName)) return;

        var startLoop = evt.Gameloop - UnitData[unitName] * 16;

        _replayActionDict[playerName].Add(new LReplayAction
        {
            UnitName = unitName,
            Gameloop = startLoop,
            Time = (int)Math.Floor(startLoop / 22.4),
            Abbr = unitAbbr,
            Supply = $"{_supplyCostDict[playerName]}/{_supplySupportDict[playerName]}"
        });
    }

    private void HandleSUnitDoneEvent(SUnitDoneEvent evt)
    {
        foreach (var playName in _supplySupportChangeDict.Keys)
        {
            var list = _supplySupportChangeDict[playName];
            for (var j = 0; j < list.Count; j++)
            {
                var record = list[j];

                if (record.Gameloop != evt.Gameloop) continue; // 不在要处理的gameloop，跳过
                if (record.Handled) continue; // 已处理，跳过

                _supplySupportDict[playName] += record.Delta;
                record.Handled = true; // 标记为已处理，避免循环中操作List
            }
        }
    }

    private static List<LReplayAction> AdjustTime(List<LReplayAction> list)
    {
        if (list.Count <= 1)
            return list;

        // Step 1: 按 (Time, UnitName) 分组，合并同单位同时间的数量
        var grouped = new Dictionary<(int Time, string UnitName), LReplayAction>();
        foreach (var action in list)
        {
            var key = (action.Time, action.UnitName);
            if (grouped.TryGetValue(key, out var existing))
                existing.Number++;  // 同单位同时间，合并数量
            else
                grouped[key] = action;
        }

        // Step 2: 按时间排序，不同单位强制错开
        var result = grouped.Values.OrderBy(a => a.Time).ToList();

        for (var i = 1; i < result.Count; i++)
        {
            // 只有不同单位且时间相同才需要错开
            if (result[i].Time <= result[i - 1].Time && result[i].UnitName != result[i - 1].UnitName)
                result[i].Time = result[i - 1].Time + 1;
        }

        return result;
    }

    private static bool HasDuplicateRace(Sc2Replay sc2Replay)
    {
        var players = sc2Replay.Details!.Players;

        if (players.Count < 2)
            return false;

        var seen = new HashSet<string>();
        return players.Any(player => !seen.Add(player.Race));
    }

    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    public class Sc2DataDict
    {
        public required List<string> Ignore;
        public required Dictionary<string, int> SupplyCost;
        public required Dictionary<string, int> SupplySupport;
        public required Dictionary<string, int> Techs;
        public required List<string> TerranBuildingTypeChange;
        public required Dictionary<string, int> Units;
        public required Dictionary<string, string> UnitsDict;
        public required List<string> ZergBuildingTypeChange;
    }

    /// <summary>
    ///     待处理Done事件记录
    /// </summary>
    /// <param name="gameloop">要处理事件的gameloop</param>
    /// <param name="delta">变化量</param>
    /// <param name="handled">是否已处理，默认否</param>
    private class DoneEvtRecord(int gameloop, int delta, bool handled = false)
    {
        public int Delta = delta;
        public int Gameloop = gameloop;
        public bool Handled = handled;
    }
}