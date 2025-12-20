using TactiX_Exception;
using TactiX_I18N;

namespace TactiX_OS_Tools;

public class OSTools : IOSTools
{
    public OSTools(ILang lang, ITactiXExceptionFactory exceptionFactory)
    {
#if OS_WINDOWS
        OSes = new WindowsImpl(lang.Language, exceptionFactory);
#endif
    }

    public IOSes OSes { get; }
}