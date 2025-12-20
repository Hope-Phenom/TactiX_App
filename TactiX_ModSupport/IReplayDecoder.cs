using TactiX_Models.Tactics;

namespace TactiX_ModSupport;

public interface IReplayDecoder
{
    public Task<Dictionary<string, List<L_ReplayAction>>> DecodeReplay(string replayPath);
}