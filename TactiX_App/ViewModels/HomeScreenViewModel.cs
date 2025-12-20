using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NuGet.Versioning;
using SukiUI;
using TactiX_App.ViewModels.Page;
using TactiX_App.Views.Page;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Network;
using TactiX_ModSupport;
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels;

public partial class HomeScreenViewModel : ViewModelBase, IRecipient<MbCheckVersion>
{
    public HomeScreenViewModel(ILang lang, ILoggerContainer loggerContainer,
        INetwork network, IMessenger messenger, IosTools oSTools, IServiceProvider serviceProvider)
    {
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _network = network;
        _messenger = messenger;
        _oSes = oSTools.OSes;
        _serviceProvider = serviceProvider;
        _config = _oSes.LoadConfig();

        Language = lang.Language;

        #region 组件注册

        // 很遗憾更符合mvvm的写法，给SukiSideMenu绑定数据源
        // 套用在SukiUI上时似乎不能正常运行
        // 先保证功能正常后续再优化
        NewsViewPageControl = _serviceProvider.GetRequiredService<NewsPageView>();
        NewsViewPageControl.DataContext = _serviceProvider.GetRequiredService<NewsPageViewModel>();
        TacticsHallViewPageControl = _serviceProvider.GetRequiredService<TacticsHallPageView>();
        TacticsHallViewPageControl.DataContext = _serviceProvider.GetRequiredService<TacticsHallPageViewModel>();
        ModsManagerPageViewControl = _serviceProvider.GetRequiredService<ModsManagePageView>();
        ModsManagerPageViewControl.DataContext = _serviceProvider.GetRequiredService<ModsManagePageViewModel>();
        SettingsPageViewControl = _serviceProvider.GetRequiredService<SettingsPageView>();
        SettingsPageViewControl.DataContext = _serviceProvider.GetRequiredService<SettingsPageViewModel>();
        TacticEditorPageViewControl = _serviceProvider.GetRequiredService<TacticEditorPageView>();
        TacticEditorPageViewControl.DataContext = _serviceProvider.GetRequiredService<TacticEditorPageViewModel>();
        ReplayAnalysisPageViewControl = _serviceProvider.GetRequiredService<ReplayAnalysisPageView>();
        ReplayAnalysisPageViewControl.DataContext = _serviceProvider.GetRequiredService<ReplayAnalysisPageViewModel>();

        #endregion

        _messenger.RegisterAll(this);
    }

    #region DI容器注入

    private readonly Logger _logger;
    private readonly INetwork _network;
    private readonly IMessenger _messenger;
    private readonly IoSes _oSes;
    private readonly IServiceProvider _serviceProvider;
    private readonly LConfig _config;

    #endregion

    #region 常/变量

    public ILanguage Language { get; }

    /// <summary>
    ///     官方QQ频道
    /// </summary>
    public static string QqChatUrl => "https://pd.qq.com/s/4vr81w4yl?b=9";

    /// <summary>
    ///     项目主页
    /// </summary>
    public static string HomePageUrl => "https://sc2.east-unicorn.cn";

    /// <summary>
    ///     新闻页面组件
    /// </summary>
    public UserControl NewsViewPageControl { get; }

    /// <summary>
    ///     战术大厅页面组件
    /// </summary>
    public UserControl TacticsHallViewPageControl { get; }

    /// <summary>
    ///     MOD管理页面组件
    /// </summary>
    public UserControl ModsManagerPageViewControl { get; }

    /// <summary>
    ///     设置页面组件
    /// </summary>
    public UserControl SettingsPageViewControl { get; }

    /// <summary>
    ///     战术编辑器页面组件
    /// </summary>
    public UserControl TacticEditorPageViewControl { get; }

    /// <summary>
    ///     回放解析页面组件
    /// </summary>
    public UserControl ReplayAnalysisPageViewControl { get; }

    #endregion

    #region 版本检查

    /// <summary>
    ///     检查版本是否可用/是否需要更新
    /// </summary>
    private async Task CheckVersion()
    {
        try
        {
            var currVer = GetCurrVersion();
            var resp = await _network.Client.PostVersionControlReq(
                new NVersionControlReq
                {
                    Version = currVer
                });

            var curr = NuGetVersion.Parse(currVer);
            var lastest = NuGetVersion.Parse(resp.LastestVersion);

            if (lastest.CompareTo(curr) == 0) return;

            var messageText = resp.Banned
                ? string.Format(Language.VersionControlBanned, currVer)
                : resp.ForceUpgrade
                    ? string.Format(Language.VersionControlForceUpgrade, resp.LastestVersion)
                    : string.Format(Language.VersionControlNewVersion, currVer, resp.LastestVersion);

            var type = resp.Banned
                ? MbEnumToastType.Error
                : resp.ForceUpgrade
                    ? MbEnumToastType.Error
                    : MbEnumToastType.Info;

            var msg = new MbToastVersion
            {
                Title = Language.VersionControlToastTitle,
                Message = messageText,
                Type = type,
                ReleaseUrl = resp.ReleaseUrl
            };

            _messenger.Send(msg);
        }
        catch (Exception ex)
        {
            var msg = new MbToastVersion
            {
                Title = Language.ToastTitleError,
                Message = string.Format(Language.VersionControlError, ex.Message),
                Type = MbEnumToastType.Error,
                ReleaseUrl = string.Empty
            };

            _messenger.Send(msg);
        }
    }

    /// <summary>
    ///     获取当前运行的版本号
    /// </summary>
    /// <returns>版本号字符串-{major}.{minor}.{build}.{revision}</returns>
    private static string GetCurrVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();
        if (assemblyName == null || assemblyName.Version == null) return string.Empty;
        var version = assemblyName.Version;

        var major = version.Major;
        var minor = version.Minor;
        var build = version.Build;
        var revision = version.Revision;

        return $"{major}.{minor}.{build}.{revision}";
    }

    public void Receive(MbCheckVersion message)
    {
        Task.Run(CheckVersion);
    }

    #endregion

    #region UI事件响应

    /// <summary>
    ///     打开指定的Url
    /// </summary>
    [RelayCommand]
    public async Task OpenWebUrl(string url)
    {
        await Task.Run(() => _oSes.OpenUrl(url));
    }

    /// <summary>
    ///     打开战术准备页面
    /// </summary>
    [RelayCommand]
    public void OpenPreparePage()
    {
        try
        {
            // 检查是否进行了配置
            if (string.IsNullOrEmpty(_config.CurrentlyEnabledMod))
            {
                _messenger.Send(new MbToastPureText
                {
                    Message = Language.TacticPlayingModNotSelected,
                    Title = Language.ToastTitleError,
                    Type = MbEnumToastType.Error
                });

                return;
            }

            // 检查文件是否存在
            if (!File.Exists(_config.CurrentlyEnabledMod))
            {
                _messenger.Send(new MbToastPureText
                {
                    Message = Language.TacticPlayingModNotExists,
                    Title = Language.ToastTitleError,
                    Type = MbEnumToastType.Error
                });

                return;
            }

            // 检查能否正常读取
            using var mod = new ModPackage(_config.CurrentlyEnabledMod);
            if (mod.ModDesc == null)
            {
                _messenger.Send(new MbToastPureText
                {
                    Message = Language.TacticPlayingModFormatError,
                    Title = Language.ToastTitleError,
                    Type = MbEnumToastType.Error
                });

                return;
            }

            // 通知打开播放界面
            _messenger.Send(new MbOpenTacticPlayWindow());
        }
        catch (FileNotFoundException)
        {
            _messenger.Send(new MbToastPureText
            {
                Message = Language.TacticPlayingModFormatError,
                Title = Language.ToastTitleError,
                Type = MbEnumToastType.Error
            });
        }
        catch (Exception ex)
        {
            _messenger.Send(new MbToastPureText
            {
                Message = ex.Message,
                Title = Language.ToastTitleError,
                Type = MbEnumToastType.Error
            });
        }
    }

    #endregion
}