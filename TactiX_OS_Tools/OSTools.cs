using TactiX_Exception;
using TactiX_I18N;

namespace TactiX_OS_Tools;

public class OsTools : IosTools
{
    public OsTools(ILang lang, ITactiXExceptionFactory exceptionFactory)
    {
#if OS_WINDOWS
        OSes = new WindowsImpl(lang.Language, exceptionFactory);
#endif
    }

    public IoSes OSes { get; }
}