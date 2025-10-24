using Newtonsoft.Json;
using s2protocol.NET;
using s2protocol.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TactiX_Models.Tactics;

namespace TactiX_ModSupport
{
    public class SC2ReplayDecoder : IReplayDecoder
    {
        private const string DATA_DICT_JSON = "SC2ProductionDuration.json";

        private Dictionary<string, int> _unitData;
        private Dictionary<string, int> _techData;
        private Dictionary<int, string> _playerNames;

        private readonly ReplayDecoderOptions options;

        public SC2ReplayDecoder()
        {
            // 为了避免UI预览器崩溃因此加了过滤
            // 实际逻辑没有必要这么复杂
            if (File.Exists(DATA_DICT_JSON))
            {
                var txt = File.ReadAllText(DATA_DICT_JSON);
                var sc2data = JsonConvert.DeserializeObject<SC2DataDict>(txt);
                if (sc2data == null) throw new Exception("CANT FIND SC2ProductionDuration.json");
                _unitData = sc2data.Units;
                _techData = sc2data.Techs;
            }
            else
            {
                _unitData = new();
                _techData = new();
            }

            _playerNames = new();

            options = new ReplayDecoderOptions()
            {
                Details = true,
                Metadata = false,
                MessageEvents = false,
                TrackerEvents = true,
                GameEvents = false,
                AttributeEvents = false
            };
        }

        public async Task<Dictionary<string, List<L_ReplayAction>>> DecodeReplay(string replayPath)
        {
            try
            {
                _playerNames.Clear();

                var replayActionDict = new Dictionary<string, List<L_ReplayAction>>();

                ReplayDecoder decoder = new();
                Sc2Replay? replay = await decoder.DecodeAsync(replayPath, options);

                if (replay == null
                    || replay.Details == null
                    || replay.TrackerEvents == null) return new Dictionary<string, List<L_ReplayAction>>();

                var playerIndex = 1;
                foreach (var player in replay.Details.Players)
                {
                    _playerNames.Add(playerIndex, player.Name);
                    playerIndex++;
                }

                foreach (var evt in replay.TrackerEvents.SUnitInitEvents)
                {
                    if (evt.Gameloop == 0) continue;

                    var playerName = _playerNames[evt.ControlPlayerId];

                    if (!replayActionDict.ContainsKey(playerName)) replayActionDict.Add(playerName, new());

                    replayActionDict[playerName].Add(new L_ReplayAction()
                    {
                        UnitName = evt.UnitTypeName,
                        Gameloop = evt.Gameloop,
                        Time = (int)Math.Floor(evt.Gameloop / 22.4)
                    });
                }

                foreach (var evt in replay.TrackerEvents.SUnitBornEvents)
                {
                    if (evt.Gameloop == 0) continue;

                    var playerName = _playerNames[evt.ControlPlayerId];

                    if (!replayActionDict.ContainsKey(playerName)) replayActionDict.Add(playerName, new());

                    var name = evt.UnitTypeName;
                    if (_unitData.ContainsKey(name))
                    {
                        var startLoop = evt.Gameloop - _unitData[name] * 16;

                        replayActionDict[playerName].Add(new L_ReplayAction()
                        {
                            UnitName = evt.UnitTypeName,
                            Gameloop = startLoop,
                            Time = (int)Math.Floor(startLoop / 22.4)
                        });
                    }
                }

                foreach (var evt in replay.TrackerEvents.SUpgradeEvents)
                {
                    if (evt.Gameloop == 0) continue;

                    var playerName = _playerNames[evt.PlayerId];

                    if (!replayActionDict.ContainsKey(playerName)) replayActionDict.Add(playerName, new());

                    var name = evt.UpgradeTypeName;
                    if (_techData.ContainsKey(name))
                    {
                        var startLoop = evt.Gameloop - _techData[name] * 16;

                        replayActionDict[playerName].Add(new L_ReplayAction()
                        {
                            UnitName = evt.UpgradeTypeName,
                            Gameloop = startLoop,
                            Time = (int)Math.Floor(startLoop / 22.4)
                        });
                    }
                }

                foreach (var playerName in replayActionDict.Keys)
                {
                    replayActionDict[playerName] = replayActionDict[playerName]
                        .OrderBy(e => e.Gameloop)
                        .ToList();
                }

                return replayActionDict;
            }
            catch
            {
                return new Dictionary<string, List<L_ReplayAction>>();
            }
        }

        public class SC2DataDict
        {
            public required Dictionary<string, int> Units;
            public required Dictionary<string, int> Techs;
        }
    }
}
