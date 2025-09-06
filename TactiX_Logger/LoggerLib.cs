using NLog;
using NLog.Config;

namespace TactiX_Logger
{
    public class LoggerLib
    {
        #region 单例模式
        private static readonly Lazy<LoggerLib> _lazyInstance = new Lazy<LoggerLib>(() => new LoggerLib(), isThreadSafe: true);
        public static LoggerLib Instance => _lazyInstance.Value;
        private LoggerLib() 
        {
            Builder = LogManager.Setup().LoadConfigurationFromFile();
        }
        #endregion

        public ISetupBuilder Builder { get; private set; }
    }
}
