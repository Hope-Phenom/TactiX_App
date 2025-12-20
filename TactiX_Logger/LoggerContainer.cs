using NLog;
using NLog.Config;

namespace TactiX_Logger;

public class LoggerContainer : ILoggerContainer
{
    public LoggerContainer()
    {
        Builder = LogManager.Setup().LoadConfigurationFromFile();
    }

    public ISetupBuilder Builder { get; }
}