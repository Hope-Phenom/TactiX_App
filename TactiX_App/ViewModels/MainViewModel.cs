namespace TactiX_App.ViewModels;

using TactiX_App.Service;
using TactiX_Models;

public partial class MainViewModel : ViewModelBase
{
    #region 常量
    #endregion

    #region 变量
    public INavigationService Navigation { get; private set; }
    #endregion

    public MainViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        Navigation.NavigateTo<LicenseViewModel>();
    }
}
