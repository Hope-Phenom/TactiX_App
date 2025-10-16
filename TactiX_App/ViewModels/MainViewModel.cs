namespace TactiX_App.ViewModels;

using Avalonia.Interactivity;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using SukiUI;
using SukiUI.Controls;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using System;
using TactiX_App.Service;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Network;
using TactiX_OS_Tools;

public partial class MainViewModel : ViewModelBase, IRecipient<MB_NavigationTo>, IRecipient<MB_NavigationBack>
{
    #region DI容器注入
    private readonly INetwork _network;
    private readonly IMessenger _messenger;
    private readonly Logger _logger;
    private readonly L_Config _config;
    private readonly IServiceProvider _serviceProvider;
    #endregion

    #region 常量
    public INavigationService Navigation { get; private set; }
    public IOSTools OSTools { get; private set; }
    public ISukiToastManager ToastManager { get; private set; }
    public ISukiDialogManager DialogManager { get; private set; }
    #endregion

    public MainViewModel(INavigationService navigation, IOSTools oSTools, INetwork network, 
        IMessenger messenger, ILoggerContainer loggerContainer, IServiceProvider serviceProvider)
    {
        OSTools = oSTools;
        Navigation = navigation;
        _network = network;
        _messenger = messenger;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _serviceProvider = serviceProvider;

        _config = OSTools.OSes.LoadConfig();

        ToastManager = new SukiToastManager();
        DialogManager = new SukiDialogManager();

        var theme = _config.NightMode
            ? ThemeVariant.Dark
            : ThemeVariant.Light;
        SukiTheme.GetInstance().ChangeBaseTheme(theme);

        if (!_config.EulaAccepted || !_config.PPAccepted)
        {
            Navigation.NavigateTo<LicenseViewModel>();
        }
        else 
        {
            Navigation.NavigateTo<HomeScreenViewModel>();
        }

        _messenger.RegisterAll(this);
    }

    public void Receive(MB_NavigationTo message)
    {
        Navigation.NavigateTo(message.NaviType);
    }

    public void Receive(MB_NavigationBack message)
    {
        Navigation.GoBack();
    }
}
