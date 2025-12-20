using System;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using SukiUI;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using TactiX_App.Service;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels;

public class MainViewModel : ViewModelBase, IRecipient<MbNavigationTo>, IRecipient<MbNavigationBack>
{
    public MainViewModel(INavigationService navigation, IosTools oSTools, INetwork network,
        IMessenger messenger, ILoggerContainer loggerContainer, IServiceProvider serviceProvider)
    {
        OsTools = oSTools;
        Navigation = navigation;
        _network = network;
        _messenger = messenger;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _serviceProvider = serviceProvider;

        _config = OsTools.OSes.LoadConfig();

        ToastManager = new SukiToastManager();
        DialogManager = new SukiDialogManager();

        if (!_config.EulaAccepted || !_config.PpAccepted)
            Navigation.NavigateTo<LicenseViewModel>();
        else
            Navigation.NavigateTo<HomeScreenViewModel>();

        _messenger.RegisterAll(this);
    }

    public void Receive(MbNavigationBack message)
    {
        Navigation.GoBack();
    }

    public void Receive(MbNavigationTo message)
    {
        Navigation.NavigateTo(message.NaviType);
    }

    #region DI容器注入

    private readonly INetwork _network;
    private readonly IMessenger _messenger;
    private readonly Logger _logger;
    private readonly LConfig _config;
    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region 常量

    public INavigationService Navigation { get; }
    public IosTools OsTools { get; }
    public ISukiToastManager ToastManager { get; private set; }
    public ISukiDialogManager DialogManager { get; private set; }

    #endregion
}