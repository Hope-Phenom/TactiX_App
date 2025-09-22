using CommunityToolkit.Mvvm.Messaging;
using NLog;
using NuGet.Versioning;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TactiX_App.Service;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models.MessageBus;
using TactiX_Network;
using static System.Net.WebRequestMethods;

namespace TactiX_App.ViewModels
{
    public class HomeScreenViewModel : ViewModelBase
    {
        #region DI容器注入
        public ILanguage Language { get; private set; }
        public INavigationService NavigationService { get; private set; }
        public Logger Logger { get; private set; }
        public INetwork Network { get; private set; }
        public IMessenger Messenger { get; private set; }
        #endregion

        public HomeScreenViewModel(ILang lang, INavigationService navigation, ILoggerContainer loggerContainer, 
            INetwork network, IMessenger messenger) 
        {
            Language = lang.Language;
            NavigationService = navigation;
            Logger = loggerContainer.Builder.GetCurrentClassLogger();
            Network = network;
            Messenger = messenger;

            Task.Run(CheckVersion);
        }

        /// <summary>
        /// 检查版本是否可用/是否需要更新
        /// </summary>
        private async Task CheckVersion()
        {
            try
            {
                var currVer = GetCurrVersion();
                var resp = await Network.Client.PostVersionControlReq(
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

                Messenger.Send(msg);
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

                Messenger.Send(msg);
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
    }
}
