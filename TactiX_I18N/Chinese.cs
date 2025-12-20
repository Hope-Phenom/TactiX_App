namespace TactiX_I18N;

public class Chinese : ILanguage
{
    public string LicenseViewBtnAccept => "接受";
    public string LicenseViewBtnDecline => "拒绝";

    public string VersionControlToastTitle => "检查程序版本...";
    public string VersionControlBanned => "当前版本[{0}]已被禁用，稍后程序将跳转至更新页面并自动退出！";
    public string VersionControlForceUpgrade => "存在强制更新版本[{0}]，稍后程序将跳转至更新页面并自动退出！";
    public string VersionControlNewVersion => "当前版本[{0}]，存在新版本[{1}]，是否跳转至下载页？";
    public string VersionControlError => "检查版本信息时发生错误，错误信息：{0}";

    public string ErrorPopupTitle => "错误信息";
    public string ErrorPopupLabelErrorCode => "错误代码：";
    public string ErrorPopupLabelErrorDesc => "错误信息：";
    public string ErrorPopupLabelFeedback => "反馈途径（Email、QQ等）：";
    public string ErrorPopupLabelUserDesc => "错误情形描述：";
    public string ErrorPopupTbxUserDesc => "请在此文本框中输入错误情形。";
    public string ErrorPopupBtnSendFeedback => "发送错误报告";
    public string ErrorPopupBtnClose => "关闭";

    public string ErrorDescTemplate => "程序发生异常，错误码：{0}，错误信息：{1}。";
    public string ErrorMuiltProcess => "重复打开了多个进程，本进程将退出！";

    public string ToastTitleError => "发生了一个错误";
    public string ButtonTxtBack => "返回";
    public string ButtonTxtSubmit => "确定";
    public string ButtonTxtRefresh => "刷新";
    public string NormalTextAvailable => "可用";
    public string NormalTextError => "错误";
    public string NormalTextDeveloping => "功能开发中，敬请期待。";
    public string NormalTextFriendshipLinks => "欢迎交换友情链接！";

    public string HomescreenSideNews => "新闻资讯";
    public string HomescreenSideTacticshall => "战术大厅";
    public string HomescreenSideReplayAnalysis => "回放解析";
    public string HomescreenSideModManangment => "MOD管理";
    public string HomescreenSideSettings => "设置";
    public string HomescreenSideEditor => "战术编辑";

    public string NewsPageViewTitleForum => "论坛热帖";
    public string NewsPageViewTitleSysNews => "系统公告";
    public string NewsPageViewTitleVideos => "视频推荐";
    public string NewsPageErrorNetwork => "由于网络问题，未能更新新闻信息";

    public string ModsManageViewLocalModsHeader => "本地Mod包";
    public string ModsManageViewLocalModsFolder => "打开本地目录";
    public string ModsManageViewLocalModsInfo => "本地检索到[{0}]个可用Mod(s)。";
    public string ModsManageViewSelectedModInfo => "选定MOD信息";
    public string ModsManageViewSelectedModAction => "MOD管理";
    public string ModsManageViewSelectedModError => "尝试读取[{0}]Mod包时发生错误，错误信息：{1}";
    public string ModsManageViewModActionLabelSelected => "当前启用MOD：";
    public string ModsManageViewModActionEnableSelected => "启用选定MOD";
    public string ModsManageViewModActionDeleteSelected => "删除选定MOD";
    public string ModsManageViewModActionDeleteSelectedError => "尝试删除[{0}]Mod包时发生错误，错误信息：{1}";

    public string TacticPlayingModNotSelected => "没有选定MOD文件，无法打开战术播放界面。请前往MOD管理选项卡进行配置。";
    public string TacticPlayingModNotExists => "配置的MOD文件不存在，请前往MOD管理选项卡进行检查。";
    public string TacticPlayingModFormatError => "尝试加载的MOD文件格式不正确，请前往MOD管理选项卡进行检查。";
    public string TacticPlayingLabelFileSelected => "选择战术文件：";
    public string TacticPlayingLabelCurrRealTimestamp => "运行时间：";
    public string TacticPlayingLabelCurrStepTimestamp => "标准时间：";
    public string TacticPlayingHotkeyAlreadyExsits => "热键[{0}+{1}]已被占用，请检查是否与其他程序存在冲突！";
    public string TacticPlayingOpacity => "战术播放透明度：";
    public string TacticPlayingTimelineCorrection => "启用时间轴矫正";
    public string TacticPlayingFileinfo => "战术文件介绍：";
    public string TacticPlayingBtnOpenFolder => "打开目录";
    public string TacticPlayingModitemEnable => "显示以下类型的步骤：";
    public string TacticPlayingModitemNone => "通用";
    public string TacticPlayingModitemWorker => "生产单位";
    public string TacticPlayingModitemArmy => "军事单位";
    public string TacticPlayingModitemBuilding => "建筑";
    public string TacticPlayingModitemTech => "科技";

    public string SettingsPageHeaderNormal => "通用设置";
    public string SettingsPageHeaderHotkey => "快捷键设置";
    public string SettingsPageHeaderAbout => "关于";
    public string SettingsPageNormalUpgrade => "检查更新";
    public string SettingsPageAboutRights => "使用本软件即表示您同意我们的《最终用户许可协议》和《隐私政策》。";

    public string EditorHeaderModItems => "MOD内容索引";
    public string EditorMenuHeaderFiles => "文件";
    public string EditorMenuFilesNew => "创建新的战术文件";
    public string EditorMenuFilesOpen => "打开已有战术文件";
    public string EditorMenuFilesSave => "保存当前战术文件";
    public string EditorMenuFilesSaveAs => "另存为当前战术文件";
    public string EditorMenuFilesExport => "导出战术文件(*.tactix)";
    public string EditorMenuFilesExportTo => "导出战术文件至...";
    public string EditorMenuHeaderEdit => "编辑";
    public string EditorMenuEditUndo => "撤销";
    public string EditorMenuEditRedo => "重做";
    public string EditorMenuEditInsertTemplate => "插入模板";
    public string EditorMenuHeaderHelp => "帮助";
    public string EditorMenuHelpGetHelp => "获取关于战术编辑的帮助";
    public string EditorErrorFileCantConvert => "无法导出战术文件，请检查格式是否符合要求";
    public string EditorErrorModNotSet => "Mod包未设置，编辑器无法启用补全功能";
    public string EditorExportSuccessTitle => "导出成功";
    public string EditorExportSuccessInfo => "战术文件导出成功，已导出至：{0}";

    public string ReplayAnalysisDeveloping => "Replay解析功能目前仅针对星际争霸2且功能仍处于开发中，\r\n您可以通过下方的按钮对Replay文件进行解析来快速创建战术文件。";
    public string ReplayAnalysisBtnDecodeSc2Replay => "解析Replay文件";
    public string ReplayAnalysisDecodeError => "尝试解析Replay时发生了错误，错误信息{0}";
    public string ReplayAnalysisDecodeErrorTitle => "解析Replay异常";
    public string ReplayAnalysisDecodeSuccess => "解析Replay成功，文件已导出至：{0}";
    public string ReplayAnalysisDecodeSuccessTitle => "解析成功";
}