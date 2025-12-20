using Microsoft.Extensions.DependencyInjection;
using TactiX_I18N;

namespace TactiX_Exception;

public class TactiXExceptionFactory : ITactiXExceptionFactory
{
    private readonly IServiceProvider _serviceProvider;

    public TactiXExceptionFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TactiXException Create()
    {
        return new TactiXException(_serviceProvider.GetRequiredService<ILang>());
    }

    public TactiXException Create(TactiXErrorCodes errorCode, string errorDesc)
    {
        return new TactiXException(_serviceProvider.GetRequiredService<ILang>(), errorCode, errorDesc);
    }
}