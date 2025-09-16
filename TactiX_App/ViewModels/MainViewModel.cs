namespace TactiX_App.ViewModels;

using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;

using TactiX_App.Service;
using TactiX_Models;
using TactiX_OS_Tools;

public partial class MainViewModel : ViewModelBase
{
    #region 常量
    #endregion

    #region 变量
    public INavigationService Navigation { get; private set; }
    public IOSTools OSTools { get; private set; }
    private LConfig Config { get; set; }
    #endregion

    public MainViewModel(INavigationService navigation, IOSTools oSTools)
    {
        OSTools = oSTools;
        Navigation = navigation;

        Config = OSTools.OSes.LoadConfig();

        if (!Config.EulaAccepted || !Config.PPAccepted)
        { 
            Navigation.NavigateTo<LicenseViewModel>();
        }
    }
}
