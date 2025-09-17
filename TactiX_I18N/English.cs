namespace TactiX_I18N
{
    public class English : ILanguage
    {
        public string LICENSE_VIEW_BTN_ACCEPT => "Accept";
        public string LICENSE_VIEW_BTN_DECLINE => "Decline";

        public string VERSION_CONTROL_BANNED => "The current version [{0}] has been disabled. The program will redirect to the update page and exit automatically shortly!";
        public string VERSION_CONTROL_FORCE_UPGRADE => "A mandatory update version [{0}] is available. The program will redirect to the update page and exit automatically shortly!";
        public string VERSION_CONTROL_NEW_VERSION => "The current version is [{0}], and a new version [{1}] is available. Would you like to proceed with the update?";
        public string VERSION_CONTROL_ERROR => "An error occurred while checking the version information. Error message: {0}";

        public string ERROR_POPUP_TITLE => "Error Message";
        public string ERROR_POPUP_LABEL_ERROR_CODE => "Error Code:";
        public string ERROR_POPUP_LABEL_ERROR_DESC => "Error Message:";
        public string ERROR_POPUP_LABEL_FEEDBACK => "Feedback channels (Email, QQ, etc.)";
        public string ERROR_POPUP_LABEL_USER_DESC => "Error scenario description:";
        public string ERROR_POPUP_TBX_USER_DESC => "Please enter the error situation in this text box.";
        public string ERROR_POPUP_BTN_SEND_FEEDBACK => "Send error report";
        public string ERROR_POPUP_BTN_CLOSE => "Close";

        public string ERROR_DESC_TEMPLATE => "An exception occurred in the program, error code: {0}, error message: {1}.";
        public string ERROR_MUILT_PROCESS => "Multiple processes have been opened repeatedly, this process will exit!";
    }
}
