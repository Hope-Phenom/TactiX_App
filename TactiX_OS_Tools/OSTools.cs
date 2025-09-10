using TactiX_Exception;
using TactiX_I18N;

namespace TactiX_OS_Tools
{
    public class OSTools : IOSTools
    {
        public IOSes OSes { get; private set; }

        public OSTools(ILang lang, ITactiXExceptionFactory exceptionFactory) 
        {
#if OS_WINDOWS
            OSes = new WindowsImpl(lang.Language, exceptionFactory);
#endif
        }
    }
}
