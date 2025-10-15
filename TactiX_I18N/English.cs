namespace TactiX_I18N
{
    public class English : ILanguage
    {
        public string LICENSE_VIEW_BTN_ACCEPT => "Accept";
        public string LICENSE_VIEW_BTN_DECLINE => "Decline";

        public string VERSION_CONTROL_TOAST_TITLE => "Checking program version...";
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

        public string TOAST_TITLE_ERROR => "An error occurred";
        public string BUTTON_TXT_BACK => "Back";
        public string BUTTON_TXT_SUBMIT => "Submit";
        public string BUTTON_TXT_REFRESH => "Refresh";

        public string HOMESCREEN_SIDE_NEWS => "News";
        public string HOMESCREEN_SIDE_TACTICSHALL => "Tactics Hall";

        public string NEWS_PAGE_VIEW_TITLE_FORUM => "Hot Topics on the Forum";
        public string NEWS_PAGE_VIEW_TITLE_SYS_NEWS => "System Announcement";
        public string NEWS_PAGE_VIEW_TITLE_VIDEOS => "Video Recommendations";
        public string NEWS_PAGE_ERROR_NETWORK => "Due to network issues, the news information could not be updated";

        public string MODS_MANAGE_VIEW_LOCAL_MODS_HEADER => "Local Mod Package(s)";
        public string MODS_MANAGE_VIEW_LOCAL_MODS_FOLDER => "Open Local Folder";
        public string MODS_MANAGE_VIEW_LOCAL_MODS_INFO => "Local search found [{0}] available Mod(s).";
        public string MODS_MANAGE_VIEW_SELECTED_MOD_INFO => "Selected MOD information";
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ACTION => "MOD Management";
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ERROR => "An error occurred while attempting to read the [{0}] mod package. Error message: {1}";
        public string MODS_MANAGE_VIEW_MOD_ACTION_LABEL_SELECTED => "Currently enabled MODs:";
        public string MODS_MANAGE_VIEW_MOD_ACTION_ENABLE_SELECTED => "Enable selected MOD";
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED => "Delete selected MOD";
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED_ERROR => "An error occurred while attempting to delete the [{0}] mod package. Error message: {1}";
    }
}
