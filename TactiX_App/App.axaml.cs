using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;

using TactiX_App.Service;
using TactiX_App.ViewModels;
using TactiX_App.ViewModels.Popup;
using TactiX_App.Views;
using TactiX_App.Views.Popup;

namespace TactiX_App;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        // 注册导航服务
        services.AddSingleton<INavigationService, NavigationService>();
        
        // 注册ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<LicenseViewModel>();
        services.AddTransient<ErrorPopupViewModel>();

        // 注册Views (Avalonia需要手动注册视图)
        services.AddTransient<MainView>();
        services.AddTransient<LicenseView>();
        services.AddTransient<ErrorPopupView>();

        var provider = services.BuildServiceProvider();
        var vm = provider.GetRequiredService<MainViewModel>();


        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
