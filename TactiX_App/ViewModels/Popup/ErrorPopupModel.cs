using System;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;

using TactiX_Logger;
using TactiX_Models;
using TactiX_Network;

namespace TactiX_App.ViewModels.Popup
{
    public partial class ErrorPopupModel : ViewModelBase
    {
        public NExceptionReportModel ReportModel { get; set; }

        [ObservableProperty]
        public bool canBeClosed;

        /// <summary>
        /// 尝试次数
        /// </summary>
        private int _times = 0;

        private Logger _logger;

        public ErrorPopupModel()
        {
            ReportModel = new();

            _logger = LoggerLib.Instance.Builder.GetCurrentClassLogger();
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
                    _logger.Info("Exception Report Upload Success.");
                    CanBeClosed = true;
                }

                if (++_times > 2)
                {
                    _logger.Warn($"Exception Report Upload Error, Info:{resp.ToString()}");
                    CanBeClosed = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PostReport Error");
            }
        }
    }
}
