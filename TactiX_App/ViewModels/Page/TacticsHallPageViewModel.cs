using NLog;
using TactiX_I18N;
using TactiX_Logger;

namespace TactiX_App.ViewModels.Page;

public class TacticsHallPageViewModel : ViewModelBase
{
    public TacticsHallPageViewModel(ILang lang, ILoggerContainer loggerContainer)
    {
        Language = lang.Language;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
    }

    #region 数据绑定

    public ILanguage Language { get; }

    #endregion

    #region DI容器注入

    private readonly Logger _logger;

    #endregion
}