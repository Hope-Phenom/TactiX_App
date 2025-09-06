using System;
using TactiX_I18N;
using TactiX_Models;

namespace TactiX_App.ViewModels.Popup
{
    public class ErrorPopupModel : ViewModelBase
    {
        public ILanguage Language => I18N.Instance.Language;

        public NExceptionReportModel ReportModel { get; set; }

        public ErrorPopupModel()
        {
            ReportModel = new();
        }
    }
}
