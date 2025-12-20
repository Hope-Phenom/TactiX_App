namespace TactiX_I18N;

public class English : ILanguage
{
    public string LicenseViewBtnAccept => "Accept";
    public string LicenseViewBtnDecline => "Decline";

    public string VersionControlToastTitle => "Checking program version...";

    public string VersionControlBanned =>
        "The current version [{0}] has been disabled. The program will redirect to the update page and exit automatically shortly!";

    public string VersionControlForceUpgrade =>
        "A mandatory update version [{0}] is available. The program will redirect to the update page and exit automatically shortly!";

    public string VersionControlNewVersion =>
        "The current version is [{0}], and a new version [{1}] is available. Do you want to jump to the download page?";

    public string VersionControlError =>
        "An error occurred while checking the version information. Error message: {0}";

    public string ErrorPopupTitle => "Error Message";
    public string ErrorPopupLabelErrorCode => "Error Code:";
    public string ErrorPopupLabelErrorDesc => "Error Message:";
    public string ErrorPopupLabelFeedback => "Feedback channels (Email, QQ, etc.)";
    public string ErrorPopupLabelUserDesc => "Error scenario description:";
    public string ErrorPopupTbxUserDesc => "Please enter the error situation in this text box.";
    public string ErrorPopupBtnSendFeedback => "Send error report";
    public string ErrorPopupBtnClose => "Close";

    public string ErrorDescTemplate => "An exception occurred in the program, error code: {0}, error message: {1}.";
    public string ErrorMuiltProcess => "Multiple processes have been opened repeatedly, this process will exit!";

    public string ToastTitleError => "An error occurred";
    public string ButtonTxtBack => "Back";
    public string ButtonTxtSubmit => "Submit";
    public string ButtonTxtRefresh => "Refresh";
    public string NormalTextAvailable => "Available";
    public string NormalTextError => "Error";
    public string NormalTextDeveloping => "Feature under development, stay tuned.";
    public string NormalTextFriendshipLinks => "Welcome to exchange friendship links!";

    public string HomescreenSideNews => "News";
    public string HomescreenSideTacticshall => "Tactics Hall";
    public string HomescreenSideReplayAnalysis => "Replay Analysis";
    public string HomescreenSideModManangment => "MOD Management";
    public string HomescreenSideSettings => "Settings";
    public string HomescreenSideEditor => "Tactic Editor";

    public string NewsPageViewTitleForum => "Hot Topics on the Forum";
    public string NewsPageViewTitleSysNews => "System Announcement";
    public string NewsPageViewTitleVideos => "Video Recommendations";
    public string NewsPageErrorNetwork => "Due to network issues, the news information could not be updated";

    public string ModsManageViewLocalModsHeader => "Local Mod Package(s)";
    public string ModsManageViewLocalModsFolder => "Open Local Folder";
    public string ModsManageViewLocalModsInfo => "Local search found [{0}] available Mod(s).";
    public string ModsManageViewSelectedModInfo => "Selected MOD information";
    public string ModsManageViewSelectedModAction => "MOD Management";

    public string ModsManageViewSelectedModError =>
        "An error occurred while attempting to read the [{0}] mod package. Error message: {1}";

    public string ModsManageViewModActionLabelSelected => "Currently enabled MODs:";
    public string ModsManageViewModActionEnableSelected => "Enable selected MOD";
    public string ModsManageViewModActionDeleteSelected => "Delete selected MOD";

    public string ModsManageViewModActionDeleteSelectedError =>
        "An error occurred while attempting to delete the [{0}] mod package. Error message: {1}";

    public string TacticPlayingModNotSelected =>
        "No MOD file selected, unable to open the tactical playback interface. Please go to the MOD Management tab to configure.";

    public string TacticPlayingModNotExists =>
        "The configured MOD file does not exist, please check in the MOD management tab.";

    public string TacticPlayingModFormatError =>
        "The MOD file you attempted to load is in an incorrect format. Please check it in the MOD management tab.";

    public string TacticPlayingLabelFileSelected => "Select Tactical File:";
    public string TacticPlayingLabelCurrRealTimestamp => "Running time:";
    public string TacticPlayingLabelCurrStepTimestamp => "Standing Time:";

    public string TacticPlayingHotkeyAlreadyExsits =>
        "The hotkey [{0}+{1}] is already in use. Please check for conflicts with other programs!";

    public string TacticPlayingOpacity => "Tactical Playback Transparency:";
    public string TacticPlayingTimelineCorrection => "Enable timeline correction";
    public string TacticPlayingFileinfo => "Tactical Document Introduction:";
    public string TacticPlayingBtnOpenFolder => "Folder";
    public string TacticPlayingModitemEnable => "Show steps of the following types:";
    public string TacticPlayingModitemNone => "Normal";
    public string TacticPlayingModitemWorker => "Worker Unit";
    public string TacticPlayingModitemArmy => "Military Unit";
    public string TacticPlayingModitemBuilding => "Building";
    public string TacticPlayingModitemTech => "Tech";

    public string SettingsPageHeaderNormal => "General Settings";
    public string SettingsPageHeaderHotkey => "Shortcut Key Settings";
    public string SettingsPageHeaderAbout => "About";
    public string SettingsPageNormalUpgrade => "Check for updates";

    public string SettingsPageAboutRights =>
        "By using this software, you agree to our End User License Agreement and Privacy Policy.";

    public string EditorHeaderModItems => "MOD Contents";
    public string EditorMenuHeaderFiles => "Files";
    public string EditorMenuFilesNew => "Create";
    public string EditorMenuFilesOpen => "Open";
    public string EditorMenuFilesSave => "Save";
    public string EditorMenuFilesSaveAs => "Save As";
    public string EditorMenuFilesExport => "Export as Tactical File (*.tactix)";
    public string EditorMenuFilesExportTo => "Export to ...";
    public string EditorMenuHeaderEdit => "Edit";
    public string EditorMenuEditUndo => "Undo";
    public string EditorMenuEditRedo => "Redo";
    public string EditorMenuEditInsertTemplate => "Insert Template";
    public string EditorMenuHeaderHelp => "Help";
    public string EditorMenuHelpGetHelp => "Get help with tactical editing";

    public string EditorErrorFileCantConvert =>
        "Failed to export the tactic file, please check if the format meets the requirements";

    public string EditorErrorModNotSet =>
        "The mod package is not set up, the editor cannot enable the completion feature";

    public string EditorExportSuccessTitle => "Export successful";
    public string EditorExportSuccessInfo => "Tactical file exported successfully, exported to: {0}";

    public string ReplayAnalysisDeveloping =>
        "The replay analysis feature is currently only available for StarCraft II and is still under development. \r\nYou can use the button below to analyze replay files and quickly create tactical documents.";

    public string ReplayAnalysisBtnDecodeSc2Replay => "Analyzing Replay Files";

    public string ReplayAnalysisDecodeError =>
        "An error occurred while trying to parse the Replay, error message {0}";

    public string ReplayAnalysisDecodeErrorTitle => "Analyzing Replay Anomalies";
    public string ReplayAnalysisDecodeSuccess => "Replay parsed successfully, file exported to: {0}";
    public string ReplayAnalysisDecodeSuccessTitle => "Analysis successful";
}