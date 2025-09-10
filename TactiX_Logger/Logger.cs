using NLog;
using NLog.Config;

namespace TactiX_Logger
{
    public class Logger : ILogger
    {
        public ISetupBuilder Builder { get; private set; }

        private Logger() 
        {
            Builder = LogManager.Setup().LoadConfigurationFromFile();
        }
    }
}
