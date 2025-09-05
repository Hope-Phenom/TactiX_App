using TactiX_I18N;

namespace TactiX_App.ViewModels.Popup
{
    public class ErrorPopupModel : ViewModelBase
    {
        public ILanguage Language => I18N.Instance.Language;
        public int ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public string? UserFeedback { get; set; }

        public ErrorPopupModel()
        {
            ErrorDesc = string.Empty;
        }
    }
}
