using NLog.Config;

namespace TactiX_Logger
{
    public interface ILogger
    {
        public ISetupBuilder Builder { get; }
    }
}
