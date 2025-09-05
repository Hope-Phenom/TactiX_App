using System.Diagnostics;
using System.Reflection;

using Tactix_Exception;
using TactiX_I18N;

namespace TactiX_OS_Tools
{
    public class Windows : IOSes
    {
        private ILanguage Language => I18N.Instance.Language;

        public string UserDataPath => throw new NotImplementedException();

        public bool IsSingleton
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyName = assembly.GetName().Name ?? string.Empty;
                Process[] app = Process.GetProcessesByName(assemblyName);
                if (app.Length > 1)
                {
                    throw new TactiXException(TactiXErrorCodes.ERROR_MUILT_PROCESS, Language.ERROR_MUILT_PROCESS);
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
