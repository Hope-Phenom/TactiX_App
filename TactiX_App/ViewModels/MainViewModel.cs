namespace TactiX_App.ViewModels;

using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using SukiUI.Controls;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using TactiX_App.Service;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Network;
using TactiX_OS_Tools;

public partial class MainViewModel : ViewModelBase
{
    #region DI容器注入
    public INavigationService Navigation { get; private set; }
    public IOSTools OSTools { get; private set; }
    public INetwork Network { get; private set; }
    public IMessenger Messenger { get; private set; }
    public Logger Logger { get; private set; }
    private L_Config Config { get; set; }
    #endregion

    #region 常量
    public ISukiToastManager ToastManager { get; private set; }
    public ISukiDialogManager DialogManager { get; private set; }
    #endregion

    public MainViewModel(INavigationService navigation, IOSTools oSTools, INetwork network, IMessenger messenger, ILoggerContainer loggerContainer)
    {
        OSTools = oSTools;
        Navigation = navigation;
        Network = network;
        Messenger = messenger;
        Logger = loggerContainer.Builder.GetCurrentClassLogger();

        Config = OSTools.OSes.LoadConfig();

        ToastManager = new SukiToastManager();
        DialogManager = new SukiDialogManager();

        if (!Config.EulaAccepted || !Config.PPAccepted)
        {
            Navigation.NavigateTo<LicenseViewModel>();
        }
        else 
        {
            Navigation.NavigateTo<HomeScreenViewModel>();
        }
    }
}
