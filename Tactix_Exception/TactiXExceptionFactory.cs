using Microsoft.Extensions.DependencyInjection;
using TactiX_Localization;

namespace TactiX_Exception;

public class TactiXExceptionFactory : ITactiXExceptionFactory
{
    private readonly ILocalizationService _localizationService;

    public TactiXExceptionFactory(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public TactiXException Create()
    {
        return new TactiXException(_localizationService);
    }

    public TactiXException Create(TactiXErrorCodes errorCode, string errorDesc)
    {
        return new TactiXException(_localizationService, errorCode, errorDesc);
    }
}