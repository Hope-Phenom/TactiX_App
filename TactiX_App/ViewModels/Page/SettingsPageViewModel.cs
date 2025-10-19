using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

using TactiX_Logger;

namespace TactiX_App.ViewModels.Page
{
    public class SettingsPageViewModel : ViewModelBase
    {
        #region DI容器注入
        private readonly Logger _logger;
        #endregion

        public SettingsPageViewModel(ILoggerContainer loggerContainer)
        {
            _logger = loggerContainer.Builder.GetCurrentClassLogger();
        }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public SettingsPageViewModel() { } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif
    }
}
