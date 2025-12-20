using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SukiUI;
using TactiX_App.Service;
using TactiX_App.ViewModels;
using TactiX_App.ViewModels.Page;
using TactiX_App.ViewModels.Popup;
using TactiX_App.Views;
using TactiX_App.Views.Component;
using TactiX_App.Views.Page;
using TactiX_App.Views.Popup;
using TactiX_Exception;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_ModSupport;
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 设置暗主题和本地化
        var sukiTheme = SukiTheme.GetInstance();
        sukiTheme.ChangeBaseTheme(ThemeVariant.Dark);
        sukiTheme.Locale = CultureInfo.InstalledUICulture;
        
        var services = new ServiceCollection();
        var messenger = WeakReferenceMessenger.Default;

        // 注册服务
        services.AddSingleton<ILoggerContainer, LoggerContainer>();
        services.AddSingleton<ILang, Lang>();
        services.AddSingleton<ITactiXExceptionFactory, TactiXExceptionFactory>();
        services.AddSingleton<IosTools, OsTools>();
        services.AddSingleton<INetwork, Network>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IMessenger>(messenger);
        services.AddSingleton<ITactiXSourceEncoder, TactiXSourceEncoder>();
        services.AddSingleton<IReplayDecoder, Sc2ReplayDecoder>();
        services.AddSingleton<ILocalizationService, LocalizationService>();

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
        services.AddTransient<TacticEditorPageViewModel>();
        services.AddTransient<ReplayAnalysisPageViewModel>();

        // 注册Views (Avalonia需要手动注册视图)
        services.AddTransient<MainWindow>();
        services.AddTransient<MainView>();
        services.AddTransient<LicenseView>();
        services.AddTransient<ErrorPopupView>();
        services.AddTransient<HomeScreenView>();
        services.AddTransient<NewsPageView>();
        services.AddTransient<TacticsHallPageView>();
        services.AddTransient<ModsManagePageView>();
        services.AddTransient<TacticPlayWindow>();
        services.AddTransient<SettingsPageView>();
        services.AddTransient<TacticEditorPageView>();
        services.AddTransient<ReplayAnalysisPageView>();

        // 注册其他UI组件
        services.AddTransient<KeyMapItem>();

        var provider = services.BuildServiceProvider();
        
        var localizationService = provider.GetRequiredService<ILocalizationService>();
        localizationService.ChangeLanguage(CultureInfo.InstalledUICulture.Name);
        
        var vm = provider.GetRequiredService<MainViewModel>();

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        DataContext = vm;

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
            {
                var mainWindow = provider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = vm;
            
                desktop.MainWindow = mainWindow;
                break;
            }
            case ISingleViewApplicationLifetime singleViewPlatform:
            {
                var mainView = provider.GetRequiredService<MainView>();
                mainView.DataContext = vm;
            
                singleViewPlatform.MainView = mainView;
                break;
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}