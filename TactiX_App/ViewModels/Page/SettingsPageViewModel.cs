using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using SukiUI.Controls;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models.MessageBus;

namespace TactiX_App.ViewModels.Page
{
    public class SettingsPageViewModel : ViewModelBase
    {
        #region DI容器注入
        public ILanguage Language { get; private set; }
        private readonly Logger _logger;
        private readonly IMessenger _messenger;
        #endregion

        #region 数据绑定
        #endregion

        public SettingsPageViewModel(ILoggerContainer loggerContainer, ILang lang, IMessenger messenger)
        {
            Language = lang.Language;

            _logger = loggerContainer.Builder.GetCurrentClassLogger();
            _messenger = messenger;

            // SukiUI的SettingsLayout存在Bug，SettingsLayoutItems的Header
            // 如果设置了Header的数据绑定，Item将失效，因此只能通过消息手动更新
            UpdateHeader();
        }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public SettingsPageViewModel() { } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

        /// <summary>
        /// 更新Header文本
        /// </summary>
        private void UpdateHeader()
        {
            _messenger.Send(new MB_FIX_SettingsLayoutItemsHeader()
            {
                HeaderText = Language.SETTINGS_PAGE_HEADER_NORMAL,
                Name = "Normal"
            });

            _messenger.Send(new MB_FIX_SettingsLayoutItemsHeader()
            {
                HeaderText = Language.SETTINGS_PAGE_HEADER_HOTKEY,
                Name = "Hotkey"
            });

            _messenger.Send(new MB_FIX_SettingsLayoutItemsHeader()
            {
                HeaderText = Language.SETTINGS_PAGE_HEADER_ABOUT,
                Name = "About"
            });
        }
    }
}
