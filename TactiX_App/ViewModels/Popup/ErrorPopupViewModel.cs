using System;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models.MessageBus;
using TactiX_Models.Network;
using TactiX_Network;
using ILoggerContainer = TactiX_Logger.ILoggerContainer;

namespace TactiX_App.ViewModels.Popup
{
    public partial class ErrorPopupViewModel : ViewModelBase
    {
        #region DI容器注入
        public ILanguage Language { get; private set; }
        private readonly INetwork _network;
        private Logger _logger;
        private IMessenger _messenger;
        #endregion

        private const string WINDOW_NAME = "ErrorPopupView";
        public N_ExceptionReportModel ReportModel { get; set; }

        /// <summary>
        /// 尝试次数
        /// </summary>
        private int _times = 0;

        public ErrorPopupViewModel(ILoggerContainer logger, ILang lang, INetwork network, IMessenger messenger)
        {
            Language = lang.Language;

            _network = network;
            _logger = logger.Builder.GetCurrentClassLogger();
            _messenger = messenger;

            ReportModel = new();
        }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public ErrorPopupViewModel()        // 此构造函数仅用于保证窗体浏览正常
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        {
            ReportModel = new();
        }
#endif

        [RelayCommand]
        public async Task PostReport()
        {
            try
            {
                ReportModel.Create_Time = DateTime.Now;
                var resp = await _network.Client.PostExceptionReportModel(ReportModel);

                if (resp.IsSuccessStatusCode)
                {
                    _logger.Info("Exception Report Upload Success.");
                    _messenger.Send(new MB_WindowClose()
                    {
                        Name = WINDOW_NAME
                    });
                    return;
                }

                if (++_times > 2)
                {
                    _logger.Warn($"Exception Report Upload Error, Info:{resp.ToString()}");
                    _messenger.Send(new MB_WindowClose()
                    {
                        Name = WINDOW_NAME
                    });
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PostReport Error");
            }
        }
    }
}
