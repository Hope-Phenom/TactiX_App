using System;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Network;
using ILogger = TactiX_Logger.ILogger;

namespace TactiX_App.ViewModels.Popup
{
    public partial class ErrorPopupViewModel : ViewModelBase
    {
        public ILanguage Language { get; set; }
        public NExceptionReportModel ReportModel { get; set; }

        [ObservableProperty]
        public bool canBeClosed;

        /// <summary>
        /// 尝试次数
        /// </summary>
        private int _times = 0;

        private NLog.Logger Logger { get; set; }

        public ErrorPopupViewModel(ILogger logger, ILang lang)
        {
            ReportModel = new();

            Language = lang.Language;
            Logger = logger.Builder.GetCurrentClassLogger();
        }

        [RelayCommand]
        public async Task PostReport()
        {
            try
            {
                ReportModel.Create_Time = DateTime.Now;
                var resp = await Network.Instance.Client.PostExceptionReportModel(ReportModel);

                if (resp.IsSuccessStatusCode)
                {
                    Logger.Info("Exception Report Upload Success.");
                    CanBeClosed = true;
                }

                if (++_times > 2)
                {
                    Logger.Warn($"Exception Report Upload Error, Info:{resp.ToString()}");
                    CanBeClosed = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "PostReport Error");
            }
        }
    }
}
