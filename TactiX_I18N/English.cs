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
        public string NORMAL_TEXT_AVAILABLE => "Available";
        public string NORMAL_TEXT_ERROR => "Error";
        public string NORMAL_TEXT_DEVELOPING => "Feature under development, stay tuned.";

        public string HOMESCREEN_SIDE_NEWS => "News";
        public string HOMESCREEN_SIDE_TACTICSHALL => "Tactics Hall";
        public string HOMESCREEN_SIDE_REPLAY_ANALYSIS => "Replay Analysis";
        public string HOMESCREEN_SIDE_MOD_MANANGMENT => "MOD Management";
        public string HOMESCREEN_SIDE_SETTINGS => "Settings";
        public string HOMESCREEN_SIDE_EDITOR => "Tactic Editor";

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

        public string TACTIC_PLAYING_MOD_NOT_SELECTED => "No MOD file selected, unable to open the tactical playback interface. Please go to the MOD Management tab to configure.";
        public string TACTIC_PLAYING_MOD_NOT_EXISTS => "The configured MOD file does not exist, please check in the MOD management tab.";
        public string TACTIC_PLAYING_MOD_FORMAT_ERROR => "The MOD file you attempted to load is in an incorrect format. Please check it in the MOD management tab.";
        public string TACTIC_PLAYING_LABEL_FILE_SELECTED => "Select Tactical File:";
        public string TACTIC_PLAYING_LABEL_CURR_REAL_TIMESTAMP => "Running time:";
        public string TACTIC_PLAYING_LABEL_CURR_STEP_TIMESTAMP => "Step Time:";
        public string TACTIC_PLAYING_HOTKEY_ALREADY_EXSITS => "The hotkey [{0}+{1}] is already in use. Please check for conflicts with other programs!";
        public string TACTIC_PLAYING_OPACITY => "Tactical Playback Transparency:";
        public string TACTIC_PLAYING_TIMELINE_CORRECTION => "Enable timeline correction";
        public string TACTIC_PLAYING_FILEINFO => "Tactical Document Introduction:";

        public string SETTINGS_PAGE_HEADER_NORMAL => "General Settings";
        public string SETTINGS_PAGE_HEADER_HOTKEY => "Shortcut Key Settings";
        public string SETTINGS_PAGE_HEADER_ABOUT => "About";
        public string SETTINGS_PAGE_NORMAL_UPGRADE => "Check for updates";
        public string SETTINGS_PAGE_ABOUT_RIGHTS => "By using this software, you agree to our End User License Agreement and Privacy Policy.";

        public string EDITOR_HEADER_MOD_ITEMS => "MOD Contents";
        public string EDITOR_MENU_HEADER_FILES => "Files";
        public string EDITOR_MENU_FILES_NEW => "Create";
        public string EDITOR_MENU_FILES_OPEN => "Open";
        public string EDITOR_MENU_FILES_SAVE => "Save";
        public string EDITOR_MENU_FILES_SAVE_AS => "Save As";
        public string EDITOR_MENU_FILES_EXPORT => "Export as Tactical File (*.tactix)";
        public string EDITOR_MENU_HEADER_EDIT => "Edit";
        public string EDITOR_MENU_EDIT_UNDO => "Undo";
        public string EDITOR_MENU_EDIT_REDO => "Redo";
        public string EDITOR_MENU_EDIT_INSERT_TEMPLATE => "Insert Template";
        public string EDITOR_MENU_HEADER_HELP => "Help";
        public string EDITOR_MENU_HELP_GET_HELP => "Get help with tactical editing";
        public string EDITOR_ERROR_FILE_CANT_CONVERT => "Failed to export the tactic file, please check if the format meets the requirements";
        public string EDITOR_ERROR_MOD_NOT_SET => "The mod package is not set up, the editor cannot enable the completion feature";

        public string REPLAY_ANALYSIS_DEVELOPING => "The replay analysis feature is currently only available for StarCraft II and is still under development. \r\nYou can use the button below to analyze replay files and quickly create tactical documents.";
        public string REPLAY_ANALYSIS_BTN_DECODE_SC2REPLAY => "Analyzing Replay Files";
        public string REPLAY_ANALYSIS_DECODE_ERROR => "An error occurred while trying to parse the Replay, error message {0}";
        public string REPLAY_ANALYSIS_DECODE_ERROR_TITLE => "Analyzing Replay Anomalies";
        public string REPLAY_ANALYSIS_DECODE_SUCCESS => "Replay parsed successfully, file exported to: {0}";
        public string REPLAY_ANALYSIS_DECODE_SUCCESS_TITLE => "Analysis successful";
    }
}
