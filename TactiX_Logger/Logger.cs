using NLog;
using NLog.Config;

namespace TactiX_Logger
{
    public class Logger : ILoggerContainer
    {
        public ISetupBuilder Builder { get; private set; }

        private Logger() 
        {
            Builder = LogManager.Setup().LoadConfigurationFromFile();
        }
    }
}
