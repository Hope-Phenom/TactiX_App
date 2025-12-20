using TactiX_Exception;
using TactiX_Localization;

namespace TactiX_OS_Tools;

public class OsTools : IosTools
{
    public OsTools(ILocalizationService localizationService, ITactiXExceptionFactory exceptionFactory)
    {
#if OS_WINDOWS
        OSes = new WindowsImpl(localizationService, exceptionFactory);
#endif
    }

    public IoSes OSes { get; }
}