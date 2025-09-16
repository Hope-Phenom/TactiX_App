using NLog.Config;

namespace TactiX_Logger
{
    public interface ILoggerContainer
    {
        public ISetupBuilder Builder { get; }
    }
}
