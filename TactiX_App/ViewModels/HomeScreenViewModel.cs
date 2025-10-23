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
using TactiX_App.Views;
using TactiX_App.Views.Page;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_ModSupport;
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels
{
    public partial class HomeScreenViewModel : ViewModelBase, IRecipient<MB_CheckVersion>
    {
        #region DI容器注入
        private readonly Logger _logger;
        private readonly INetwork _network;
        private readonly IMessenger _messenger;
        private readonly IOSes _oSes;
        private readonly IServiceProvider _serviceProvider;
        private readonly L_Config _config;
        #endregion

        #region 常/变量
        public ILanguage Language { get; private set; }
        /// <summary>
        /// 官方QQ频道
        /// </summary>
        public string QqChatUrl => "https://pd.qq.com/s/4vr81w4yl?b=9";
        /// <summary>
        /// 项目主页
        /// </summary>
        public string HomePageUrl => "https://sc2.east-unicorn.cn";
        /// <summary>
        /// 新闻页面组件
        /// </summary>
        public UserControl NewsViewPageControl { get; private set; }
        /// <summary>
        /// 战术大厅页面组件
        /// </summary>
        public UserControl TacticsHallViewPageControl { get; private set; }
        /// <summary>
        /// MOD管理页面组件
        /// </summary>
        public UserControl ModsManagerPageViewControl { get; private set; }
        /// <summary>
        /// 设置页面组件
        /// </summary>
        public UserControl SettingsPageViewControl { get; private set; }
        /// <summary>
        /// 战术编辑器页面组件
        /// </summary>
        public UserControl TacticEditorPageViewControl { get; private set; }
        /// <summary>
        /// 回放解析页面组件
        /// </summary>
        public UserControl ReplayAnalysisPageViewControl { get; private set; }
        #endregion

        public HomeScreenViewModel(ILang lang, ILoggerContainer loggerContainer, 
            INetwork network, IMessenger messenger, IOSTools oSTools, IServiceProvider serviceProvider)
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

            Task.Run(CheckVersion);
        }

        #region 版本检查
        /// <summary>
        /// 检查版本是否可用/是否需要更新
        /// </summary>
        private async Task CheckVersion()
        {
            try
            {
                var currVer = GetCurrVersion();
                var resp = await _network.Client.PostVersionControlReq(
                    new TactiX_Models.Network.N_VersionControlReq()
                    {
                        Version = currVer
                    });

                var curr = NuGetVersion.Parse(currVer);
                var lastest = NuGetVersion.Parse(resp.LastestVersion);

                if (lastest.CompareTo(curr) == 0) return;

                var messageText = resp.Banned
                    ? string.Format(Language.VERSION_CONTROL_BANNED, currVer)
                    : resp.Force_Upgrade
                        ? string.Format(Language.VERSION_CONTROL_FORCE_UPGRADE, resp.LastestVersion)
                        : string.Format(Language.VERSION_CONTROL_NEW_VERSION, currVer, resp.LastestVersion);

                var type = resp.Banned
                    ? MB_Enum_ToastType.Error
                    : resp.Force_Upgrade
                        ? MB_Enum_ToastType.Error
                        : MB_Enum_ToastType.Info;

                var msg = new MB_ToastVersion()
                {
                    Title = Language.VERSION_CONTROL_TOAST_TITLE,
                    Message = messageText,
                    Type = type,
                    Release_Url = resp.Release_Url
                };

                _messenger.Send(msg);
            }
            catch (Exception ex)
            {
                var msg = new MB_ToastVersion()
                {
                    Title = Language.TOAST_TITLE_ERROR,
                    Message = string.Format(Language.VERSION_CONTROL_ERROR, ex.Message),
                    Type = MB_Enum_ToastType.Error,
                    Release_Url = string.Empty
                };

                _messenger.Send(msg);
            }
        }

        /// <summary>
        /// 获取当前运行的版本号
        /// </summary>
        /// <returns>版本号字符串-{major}.{minor}.{build}.{revision}</returns>
        private string GetCurrVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            if (assemblyName == null || assemblyName.Version == null) return string.Empty;
            Version version = assemblyName.Version;

            var major = version.Major;
            var minor = version.Minor;
            var build = version.Build;
            var revision = version.Revision;

            return $"{major}.{minor}.{build}.{revision}";
        }

        public void Receive(MB_CheckVersion message)
        {
            Task.Run(CheckVersion);
        }
        #endregion

        #region UI事件响应
        /// <summary>
        /// 打开指定的Url
        /// </summary>
        [RelayCommand]
        public async Task OpenWebUrl(string url)
        {
            await Task.Run(() => _oSes.OpenUrl(url));
        }

        /// <summary>
        /// 切换明暗主题色
        /// </summary>
        [RelayCommand]
        public void ThemeSwtich()
        {
            _config.NightMode = !_config.NightMode;
            SukiTheme.GetInstance().SwitchBaseTheme();
            _oSes.SaveConfig();
        }

        /// <summary>
        /// 打开战术准备页面
        /// </summary>
        [RelayCommand]
        public void OpenPreparePage()
        {
            try
            {
                // 检查是否进行了配置
                if (string.IsNullOrEmpty(_config.CurrentlyEnabledMOD))
                {
                    _messenger.Send(new MB_ToastPureText()
                    {
                        Message = Language.TACTIC_PLAYING_MOD_NOT_SELECTED,
                        Title = Language.TOAST_TITLE_ERROR,
                        Type = MB_Enum_ToastType.Error
                    });

                    return;
                }

                // 检查文件是否存在
                if (!File.Exists(_config.CurrentlyEnabledMOD))
                {
                    _messenger.Send(new MB_ToastPureText()
                    {
                        Message = Language.TACTIC_PLAYING_MOD_NOT_EXISTS,
                        Title = Language.TOAST_TITLE_ERROR,
                        Type = MB_Enum_ToastType.Error
                    });

                    return;
                }

                // 检查能否正常读取
                using var mod = new ModPackage(_config.CurrentlyEnabledMOD);
                if (mod.ModDesc == null)
                {
                    _messenger.Send(new MB_ToastPureText()
                    {
                        Message = Language.TACTIC_PLAYING_MOD_FORMAT_ERROR,
                        Title = Language.TOAST_TITLE_ERROR,
                        Type = MB_Enum_ToastType.Error
                    });

                    return;
                }

                // 通知打开播放界面
                _messenger.Send(new MB_OpenTacticPlayWindow());
            }
            catch (FileNotFoundException)
            {
                _messenger.Send(new MB_ToastPureText()
                {
                    Message = Language.TACTIC_PLAYING_MOD_FORMAT_ERROR,
                    Title = Language.TOAST_TITLE_ERROR,
                    Type = MB_Enum_ToastType.Error
                });
            }
            catch (Exception ex)
            {
                _messenger.Send(new MB_ToastPureText()
                {
                    Message = ex.Message,
                    Title = Language.TOAST_TITLE_ERROR,
                    Type = MB_Enum_ToastType.Error
                });
            }
        }
        #endregion
    }
}
