using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using NLog;

using TactiX_App.Service;
using TactiX_App.ViewModels;
using TactiX_App.ViewModels.Page;
using TactiX_App.ViewModels.Popup;
using TactiX_App.Views;
using TactiX_App.Views.Page;
using TactiX_App.Views.Popup;
using TactiX_Exception;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Network;
using TactiX_OS_Tools;

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
        var messenger = WeakReferenceMessenger.Default;

        // 注册服务
        services.AddSingleton<ILoggerContainer, LoggerContainer>();
        services.AddSingleton<ILang, Lang>();
        services.AddSingleton<ITactiXExceptionFactory, TactiXExceptionFactory>();
        services.AddSingleton<IOSTools, OSTools>();
        services.AddSingleton<INetwork, Network>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IMessenger>(messenger);

        // 注册ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<LicenseViewModel>();
        services.AddTransient<ErrorPopupViewModel>();
        services.AddTransient<HomeScreenViewModel>();
        services.AddTransient<NewsPageViewModel>();
        services.AddTransient<TacticsHallPageViewModel>();
        services.AddTransient<ModsManagePageViewModel>();
        services.AddTransient<TacticPlayWindowModel>();
        services.AddTransient<SettingsPageViewModel>();

        // 注册Views (Avalonia需要手动注册视图)
        services.AddTransient<MainView>();
        services.AddTransient<LicenseView>();
        services.AddTransient<ErrorPopupView>();
        services.AddTransient<HomeScreenView>();
        services.AddTransient<NewsPageView>();
        services.AddTransient<TacticsHallPageView>();
        services.AddTransient<ModsManagePageView>();
        services.AddTransient<TacticPlayWindow>();
        services.AddTransient<SettingsPageView>();

        var provider = services.BuildServiceProvider();
        var logger = provider.GetRequiredService<ILoggerContainer>().Builder.GetCurrentClassLogger();
        var vm = provider.GetRequiredService<MainViewModel>();

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        DataContext = vm;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(provider)
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
