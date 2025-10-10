using System;
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
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels
{
    public partial class HomeScreenViewModel : ViewModelBase
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

            // 很遗憾更符合mvvm的写法套用在SukiUI上时似乎不能正常运行
            // 先保证功能正常后续再优化
            NewsViewPageControl = _serviceProvider.GetRequiredService<NewsPageView>();
            NewsViewPageControl.DataContext = _serviceProvider.GetRequiredService<NewsPageViewModel>();

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
        #endregion

        #region UI事件响应
        /// <summary>
        /// 打开指定的Url
        /// </summary>
        [RelayCommand]
        public async Task OpenWebUrl(string url)
        {
            await Task.Run(() => _oSes.OpenWeb(url));
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
        #endregion
    }
}
