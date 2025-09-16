using NLog;
using NLog.Config;

namespace TactiX_Logger
{
    public class LoggerContainer : ILoggerContainer
    {
        public ISetupBuilder Builder { get; private set; }

        public LoggerContainer() 
        {
            Builder = LogManager.Setup().LoadConfigurationFromFile();
        }
    }
}
