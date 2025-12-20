using TactiX_Models.Tactics;

namespace TactiX_ModSupport;

public interface IReplayDecoder
{
    public Task<Dictionary<string, List<LReplayAction>>> DecodeReplay(string replayPath);
}