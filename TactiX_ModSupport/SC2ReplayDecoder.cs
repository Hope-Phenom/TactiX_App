using Newtonsoft.Json;
using NLog;
using s2protocol.NET;
using s2protocol.NET.Models;
using System.Xml.Linq;
using TactiX_Logger;
using TactiX_Models.Tactics;

namespace TactiX_ModSupport
{
    public class SC2ReplayDecoder : IReplayDecoder
    {
        private const string DATA_DICT_JSON = "SC2ProductionDuration.json";

        private readonly ReplayDecoderOptions options;
        private readonly ILogger _logger;

        public readonly SC2DataDict SC2Data;

        private Dictionary<string, int> _unitData => SC2Data.Units;
        private Dictionary<string, int> _techData => SC2Data.Techs;
        private Dictionary<string, string> _unitsDict => SC2Data.UnitsDict;
        private Dictionary<string, int> _supplyCost => SC2Data.SupplyCost;
        private Dictionary<string, int> _supplySupport => SC2Data.SupplySupport;
        private List<string> _ignore => SC2Data.Ignore;
        private List<string> _terranBuildingTypeChange => SC2Data.TerranBuildingTypeChange;
        private List<string> _zergBuildingTypeChange => SC2Data.ZergBuildingTypeChange;

        private Dictionary<int, string> playerNames;
        private Dictionary<string, int> supplyCostDict;
        private Dictionary<string, int> supplySupportDict;
        /// <summary>
        /// 绷不住了，这里用元组减少代码量
        /// 依次代表 Gameloop、变化量、是否已处理(0为未处理)
        /// </summary>
        private Dictionary<string, List<(int, int, int)>> supplySupportChangeDict;
        /// <summary>
        /// 建筑的升级变形事件是不包括playerId的，因此尝试用种族分配来处理
        /// 如果种族不重复，则按照种族来分配升级的建筑，否则跳过解析
        /// </summary>
        private Dictionary<string, string> raceDict;
        private Dictionary<string, List<L_ReplayAction>> replayActionDict;


#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public SC2ReplayDecoder(ILoggerContainer loggerContainer)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        {
            _logger = loggerContainer.Builder.GetCurrentClassLogger();

            // 为了避免UI预览器崩溃因此加了过滤
            // 实际逻辑没有这么复杂
            if (File.Exists(DATA_DICT_JSON))
            {
                var txt = File.ReadAllText(DATA_DICT_JSON);
                if (string.IsNullOrEmpty(txt)) throw new Exception("CANT FIND SC2ProductionDuration.json");
                SC2Data = JsonConvert.DeserializeObject<SC2DataDict>(txt)!;
                if (SC2Data == null) throw new Exception("SC2ProductionDuration.json File is Broken");
            }

            options = new ReplayDecoderOptions()
            {
                Details = true,
                Metadata = true,
                MessageEvents = true,
                TrackerEvents = true,
                GameEvents = true,
                AttributeEvents = true
            };
        }

        public async Task<Dictionary<string, List<L_ReplayAction>>> DecodeReplay(string replayPath)
        {
            try
            {
                playerNames = new Dictionary<int, string>();
                supplyCostDict = new Dictionary<string, int>();
                supplySupportDict = new Dictionary<string, int>();
                supplySupportChangeDict = new Dictionary<string, List<(int, int, int)>>();
                raceDict = new Dictionary<string, string>(); 
                replayActionDict = new Dictionary<string, List<L_ReplayAction>>();

                ReplayDecoder decoder = new();
                Sc2Replay? replay = await decoder.DecodeAsync(replayPath, options);

                if (replay == null
                    || replay.Details == null
                    || replay.TrackerEvents == null) return new Dictionary<string, List<L_ReplayAction>>();

                var sameRace = HasDuplicateRace(replay);

                var playerIndex = 1;
                foreach (var player in replay.Details.Players)
                {
                    var playerName = $"{player.Name}_{playerIndex}";              // 规避出现同样ID（主要是人机）的情况
                    playerNames.Add(playerIndex, playerName);

                    replayActionDict.EnsureKeyExists(playerName);
                    supplyCostDict.EnsureKeyExists(playerName);
                    supplySupportDict.EnsureKeyExists(playerName);
                    supplySupportChangeDict.EnsureKeyExists(playerName);

                    supplyCostDict[playerName] = 12;
                    supplySupportDict[playerName] = player.Race switch           // 不同语言客户端的rep种族字段也会不一样
                    {
                        "Protoss" => 15,
                        "Terran" => 15,
                        "Zerg" => 14,
                        "星灵" => 15,
                        "人类" => 15,
                        "异虫" => 14,
                        _ => 15
                    };

                    if (!sameRace)
                    {
                        raceDict.Add(player.Race, playerName);
                    }

                    playerIndex++;
                }

                // 从1开始排除开局就存在的单位
                // C#版本的解析库将不同的事件解析成了不同的独立的List，导致自动计算人口更加复杂，必须人工按gameloop进行
                for (int i = 1; i < replay.Header.ElapsedGameLoops; i++)
                {
                    foreach (var playName in supplySupportChangeDict.Keys)
                    {
                        var list = supplySupportChangeDict[playName];
                        for (int j = 0; j < list.Count; j++)
                        {
                            var result = list[j];

                            if (result.Item1 != i) continue; // 不在要处理的Gameloop，跳过
                            if (result.Item3 == 1) continue; // 已处理，跳过

                            supplySupportDict[playName] += result.Item2;
                            result.Item3 = 1;                // 标记为已处理，避免循环中操作List
                        }
                    }

                    HandleSUnitInitEvents(replay.TrackerEvents.SUnitInitEvents, i);
                    HandleSUnitBornEvents(replay.TrackerEvents.SUnitBornEvents, i);
                    HandleSUpgradeEvents(replay.TrackerEvents.SUpgradeEvents, i);
                    HandleSUnitTypeChangeEvents(replay.TrackerEvents.SUnitTypeChangeEvents, i, sameRace);
                }

                foreach (var playerName in replayActionDict.Keys)
                {
                    replayActionDict[playerName] = replayActionDict[playerName]
                        .OrderBy(e => e.Gameloop)
                        .ToList();

                    AdjustTime(replayActionDict[playerName]);
                }

                return replayActionDict;
            }
            catch
            {
                return new Dictionary<string, List<L_ReplayAction>>();
            }
        }

        private void HandleSUnitInitEvents(ICollection<SUnitInitEvent> sUnitInitEvents, int gameloop)
        {
            foreach (var evt in sUnitInitEvents)
            {
                if (evt.Gameloop != gameloop) continue;
                if (evt.ControlPlayerId == 0)
                {
                    _logger.Warn(evt.UnitTypeName);
                    continue;     // 排除非玩家控制单位
                }

                var unitName = evt.UnitTypeName;
                if (_ignore.Contains(unitName)) continue;   // 过滤单位

                var playerName = playerNames[evt.ControlPlayerId];

                if (_supplyCost.ContainsKey(unitName))                            // 单位生产消耗的人口是立即的
                {
                    supplyCostDict[playerName] += _supplyCost[unitName];
                }

                if (_supplySupport.ContainsKey(unitName))                         // 人口增加的单位/建筑是完成后才生效的
                {
                    if (evt.SUnitDoneEvent != null)
                    {
                        supplySupportChangeDict[playerName]
                            .Add((evt.SUnitDoneEvent.Gameloop, _supplySupport[unitName], 0));
                    }
                }

                if (!_unitsDict.ContainsKey(unitName))
                {
                    _logger.Error($"Error Unit Name: {unitName}");
                    continue;
                }

                replayActionDict[playerName].Add(new L_ReplayAction()
                {
                    UnitName = unitName,
                    Gameloop = evt.Gameloop,
                    Time = (int)Math.Floor(evt.Gameloop / 22.4),
                    Abbr = _unitsDict[unitName],
                    Supply = $"{supplyCostDict[playerName]}/{supplySupportDict[playerName]}"
                });
            }
        }

        private void HandleSUnitBornEvents(ICollection<SUnitBornEvent> sUnitBornEvents, int gameloop)
        {
            foreach (var evt in sUnitBornEvents)
            {
                if (evt.Gameloop != gameloop) continue;
                if (evt.ControlPlayerId == 0)
                {
                    _logger.Warn(evt.UnitTypeName);
                    continue;     // 排除非玩家控制单位
                }

                var unitName = evt.UnitTypeName;
                if (_ignore.Contains(unitName)) continue;   // 过滤单位

                var playerName = playerNames[evt.ControlPlayerId];
                var startLoop = evt.Gameloop - _unitData[unitName] * 16;

                if (_supplyCost.ContainsKey(unitName))                             // 单位生产消耗的人口是立即的
                {
                    supplyCostDict[playerName] += _supplyCost[unitName];
                }

                if (_supplySupport.ContainsKey(unitName))                          // 人口增加的单位/建筑是完成后才生效的；
                {                                                                  // Born为已完成，直接增加
                    supplySupportDict[playerName] += _supplySupport[unitName];
                }

                if (!_unitData.ContainsKey(unitName))
                {
                    _logger.Error($"Error Unit Name: {unitName}");
                    continue;
                }

                replayActionDict[playerName].Add(new L_ReplayAction()
                {
                    UnitName = unitName,
                    Gameloop = startLoop,
                    Time = (int)Math.Floor(startLoop / 22.4),
                    Abbr = _unitsDict[unitName],
                    Supply = $"{supplyCostDict[playerName]}/{supplySupportDict[playerName]}"
                });
            }
        }

        private void HandleSUpgradeEvents(ICollection<SUpgradeEvent> sUpgradeEvents, int gameloop)
        {
            foreach (var evt in sUpgradeEvents)
            {
                if (evt.Gameloop != gameloop) continue;

                var upgradeName = evt.UpgradeTypeName;
                if (_ignore.Contains(upgradeName)) continue;// 过滤升级

                var playerName = playerNames[evt.PlayerId];

                if (!_techData.ContainsKey(upgradeName))
                {
                    _logger.Error($"Error Upgrade Name: {upgradeName}");
                    continue;
                }

                var startLoop = evt.Gameloop - _techData[upgradeName] * 16;

                replayActionDict[playerName].Add(new L_ReplayAction()
                {
                    UnitName = upgradeName,
                    Gameloop = startLoop,
                    Time = (int)Math.Floor(startLoop / 22.4),
                    Abbr = _unitsDict[upgradeName],
                    Supply = $"{supplyCostDict[playerName]}/{supplySupportDict[playerName]}"
                });
            }
        }

        private void HandleSUnitTypeChangeEvents(ICollection<SUnitTypeChangeEvent> sUnitTypeChangeEvents, int gameloop, bool isSameRace)
        {
            foreach (var evt in sUnitTypeChangeEvents)
            {
                if (isSameRace) break;
                if (evt.Gameloop != gameloop) continue;

                var unitName = evt.UnitTypeName;
                if (_ignore.Contains(unitName)) continue;
                if (!_unitsDict.ContainsKey(unitName))
                {
                    _logger.Error($"Error Unit Name: {unitName}");
                    continue;
                }

                string? playerName = null;

                var isTerran = _terranBuildingTypeChange.Contains(unitName);
                var isZerg = _zergBuildingTypeChange.Contains(unitName);

                if (isTerran)
                {
                    playerName = raceDict.TryGetValue("Terran", out var terranName)
                        ? terranName
                        : raceDict.TryGetValue("人类", out var chineseName)
                            ? chineseName
                            : null;
                }

                if (isZerg)
                {
                    playerName = raceDict.TryGetValue("Zerg", out var terranName)
                        ? terranName
                        : raceDict.TryGetValue("异虫", out var chineseName)
                            ? chineseName
                            : null;
                }

                if (string.IsNullOrEmpty(playerName)) continue;

                var startLoop = evt.Gameloop - _unitData[unitName] * 16;

                replayActionDict[playerName].Add(new L_ReplayAction()
                {
                    UnitName = unitName,
                    Gameloop = startLoop,
                    Time = (int)Math.Floor(startLoop / 22.4),
                    Abbr = _unitsDict[unitName],
                    Supply = $"{supplyCostDict[playerName]}/{supplySupportDict[playerName]}"
                });
            }
        }

        private List<L_ReplayAction> AdjustTime(List<L_ReplayAction> list)
        {
            if (list.Count <= 1)
                return list;

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].Time >= list[i + 1].Time)
                {
                    int j = i + 1;
                    // 处理重复值的连锁反应
                    while (j < list.Count)
                    {
                        if (list[j].Time <= list[j - 1].Time)
                        {
                            list[j].Time = list[j - 1].Time + 1;
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    // 跳过已处理的部分
                    i = j - 1;
                }
            }

            return list;
        }

        private bool HasDuplicateRace(Sc2Replay sc2Replay)
        {
            var players = sc2Replay.Details!.Players;

            if (players == null || players.Count < 2)
                return false;

            var seen = new HashSet<string>();
            foreach (var player in players)
            {
                if (!seen.Add(player.Race)) // 如果添加失败，说明已存在
                    return true;
            }
            return false;
        }

        public class SC2DataDict
        {
            public required Dictionary<string, int> Units;
            public required Dictionary<string, int> Techs;
            public required Dictionary<string, string> UnitsDict;
            public required Dictionary<string, int> SupplyCost;
            public required Dictionary<string, int> SupplySupport;
            public required List<string> Ignore;
            public required List<string> TerranBuildingTypeChange;
            public required List<string> ZergBuildingTypeChange;
        }
    }
}
