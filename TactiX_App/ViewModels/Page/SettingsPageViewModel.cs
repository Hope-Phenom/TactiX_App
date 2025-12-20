using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using TactiX_Localization;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page;

public partial class SettingsPageViewModel : ViewModelBase
{
    public SettingsPageViewModel(ILoggerContainer loggerContainer, ILocalizationService localizationService, IMessenger messenger,
        IosTools oSTools)
    {
        Config = oSTools.OSes.LoadConfig();

        _localizationService = localizationService;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _messenger = messenger;

        // SukiUI的SettingsLayout存在Bug，SettingsLayoutItems的Header
        // 如果设置了Header的数据绑定，Item将失效，因此只能通过消息手动更新
        UpdateHeader();
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public SettingsPageViewModel()
    {
    } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

    /// <summary>
    ///     更新Header文本
    /// </summary>
    private void UpdateHeader()
    {
        _messenger.Send(new MbFixSettingsLayoutItemsHeader
        {
            HeaderText = _localizationService.GetString("SettingsPageHeaderNormal"),
            Name = "Normal"
        });

        _messenger.Send(new MbFixSettingsLayoutItemsHeader
        {
            HeaderText = _localizationService.GetString("SettingsPageHeaderHotkey"),
            Name = "Hotkey"
        });

        _messenger.Send(new MbFixSettingsLayoutItemsHeader
        {
            HeaderText = _localizationService.GetString("SettingsPageHeaderAbout"),
            Name = "About"
        });
    }

    #region Command绑定

    /// <summary>
    ///     检查更新
    /// </summary>
    [RelayCommand]
    public void CheckUpgrade()
    {
        _messenger.Send(new MbCheckVersion());
    }

    #endregion

    #region DI容器注入
    
    public LConfig Config { get; private set; }
    private readonly ILocalizationService _localizationService;
    private readonly Logger _logger;
    private readonly IMessenger _messenger;

    #endregion
}