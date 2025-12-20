namespace TactiX_I18N;

/// <summary>
///     语言接口
/// </summary>
public interface ILanguage
{
    #region UI部分文本

    #region 许可协议弹窗

    /// <summary>
    ///     许可协议弹窗-按钮-接受
    /// </summary>
    public string LicenseViewBtnAccept { get; }

    /// <summary>
    ///     许可协议弹窗-按钮-拒绝
    /// </summary>
    public string LicenseViewBtnDecline { get; }

    #endregion

    #region 版本控制提示信息

    /// <summary>
    ///     版本控制提示信息-Toast信息标题
    /// </summary>
    public string VersionControlToastTitle { get; }

    /// <summary>
    ///     版本控制提示信息-当前版本已被禁用
    /// </summary>
    public string VersionControlBanned { get; }

    /// <summary>
    ///     版本控制提示信息-有强制更新
    /// </summary>
    public string VersionControlForceUpgrade { get; }

    /// <summary>
    ///     版本控制提示信息-有新版本，请考虑更新
    /// </summary>
    public string VersionControlNewVersion { get; }

    /// <summary>
    ///     版本控制提示信息-检查版本信息时发生错误
    /// </summary>
    public string VersionControlError { get; }

    #endregion

    #region 错误信息弹窗

    /// <summary>
    ///     错误信息弹窗-标题
    /// </summary>
    public string ErrorPopupTitle { get; }

    /// <summary>
    ///     错误信息弹窗-错误码
    /// </summary>
    public string ErrorPopupLabelErrorCode { get; }

    /// <summary>
    ///     错误信息弹窗-错误信息
    /// </summary>
    public string ErrorPopupLabelErrorDesc { get; }

    /// <summary>
    ///     错误信息弹窗-用户反馈渠道
    /// </summary>
    public string ErrorPopupLabelFeedback { get; }

    /// <summary>
    ///     错误信息弹窗-用户反馈的信息
    /// </summary>
    public string ErrorPopupLabelUserDesc { get; }

    /// <summary>
    ///     错误信息弹窗-用户反馈信息文本框水纹
    /// </summary>
    public string ErrorPopupTbxUserDesc { get; }

    /// <summary>
    ///     错误信息弹窗-按钮-发送反馈
    /// </summary>
    public string ErrorPopupBtnSendFeedback { get; }

    /// <summary>
    ///     错误信息弹窗-按钮-关闭
    /// </summary>
    public string ErrorPopupBtnClose { get; }

    #endregion

    #region 主页面

    /// <summary>
    ///     主页面-侧栏按钮-新闻页面
    /// </summary>
    public string HomescreenSideNews { get; }

    /// <summary>
    ///     主页面-侧栏按钮-战术大厅
    /// </summary>
    public string HomescreenSideTacticshall { get; }

    /// <summary>
    ///     主页面-侧栏按钮-回放解析
    /// </summary>
    public string HomescreenSideReplayAnalysis { get; }

    /// <summary>
    ///     主页面-侧栏按钮-MOD管理
    /// </summary>
    public string HomescreenSideModManangment { get; }

    /// <summary>
    ///     主页面-侧栏按钮-设置
    /// </summary>
    public string HomescreenSideSettings { get; }

    /// <summary>
    ///     主页面-侧边按钮-战术编辑
    /// </summary>
    public string HomescreenSideEditor { get; }

    #endregion

    #region 新闻Page相关

    /// <summary>
    ///     新闻Page-标题-论坛热帖
    /// </summary>
    public string NewsPageViewTitleForum { get; }

    /// <summary>
    ///     新闻Page-标题-系统新闻
    /// </summary>
    public string NewsPageViewTitleSysNews { get; }

    /// <summary>
    ///     新闻Page-标题-视频
    /// </summary>
    public string NewsPageViewTitleVideos { get; }

    /// <summary>
    ///     新闻Page-错误信息-网络连接
    /// </summary>
    public string NewsPageErrorNetwork { get; }

    #endregion

    #region MOD管理Page相关

    /// <summary>
    ///     MOD管理Page-本地MOD列表-HEADER
    /// </summary>
    public string ModsManageViewLocalModsHeader { get; }

    /// <summary>
    ///     MOD管理Page-打开本地目录
    /// </summary>
    public string ModsManageViewLocalModsFolder { get; }

    /// <summary>
    ///     MOD管理Page-本地MOD信息
    /// </summary>
    public string ModsManageViewLocalModsInfo { get; }

    /// <summary>
    ///     MOD管理Page-选定MOD信息-HEADER
    /// </summary>
    public string ModsManageViewSelectedModInfo { get; }

    /// <summary>
    ///     MOD管理Page-选定MOD操作
    /// </summary>
    public string ModsManageViewSelectedModAction { get; }

    /// <summary>
    ///     MOD管理Page-选定MOD操作-装载错误
    /// </summary>
    public string ModsManageViewSelectedModError { get; }

    /// <summary>
    ///     MOD管理Page-选定MOD操作-已启用MOD展示
    /// </summary>
    public string ModsManageViewModActionLabelSelected { get; }

    /// <summary>
    ///     MOD管理Page-选定MOD操作-启用选定MOD
    /// </summary>
    public string ModsManageViewModActionEnableSelected { get; }

    /// <summary>
    ///     MOD管理Page-选定MOD操作-删除选定MOD
    /// </summary>
    public string ModsManageViewModActionDeleteSelected { get; }

    /// <summary>
    ///     MOD管理Page-选定MOD操作-删除选定MOD错误
    /// </summary>
    public string ModsManageViewModActionDeleteSelectedError { get; }

    #endregion

    #region 战术播放Window相关

    /// <summary>
    ///     战术播放Window-没有选定MOD文件
    /// </summary>
    public string TacticPlayingModNotSelected { get; }

    /// <summary>
    ///     战术播放Window-MOD文件不存在
    /// </summary>
    public string TacticPlayingModNotExists { get; }

    /// <summary>
    ///     战术播放Window-MOD文件格式不正确
    /// </summary>
    public string TacticPlayingModFormatError { get; }

    /// <summary>
    ///     战术播放Window-选定战术文件
    /// </summary>
    public string TacticPlayingLabelFileSelected { get; }

    /// <summary>
    ///     战术播放Window-当前的真实时间戳
    /// </summary>
    public string TacticPlayingLabelCurrRealTimestamp { get; }

    /// <summary>
    ///     战术播放Window-当前战术步骤的时间戳
    /// </summary>
    public string TacticPlayingLabelCurrStepTimestamp { get; }

    /// <summary>
    ///     战术播放Window-热键已被占用
    /// </summary>
    public string TacticPlayingHotkeyAlreadyExsits { get; }

    /// <summary>
    ///     战术播放Window-播放时透明度
    /// </summary>
    public string TacticPlayingOpacity { get; }

    /// <summary>
    ///     战术播放Window-启用时间轴矫正
    /// </summary>
    public string TacticPlayingTimelineCorrection { get; }

    /// <summary>
    ///     战术播放Window-战术文件介绍
    /// </summary>
    public string TacticPlayingFileinfo { get; }

    /// <summary>
    ///     战术播放Window-打开目录按钮
    /// </summary>
    public string TacticPlayingBtnOpenFolder { get; }

    /// <summary>
    ///     战术播放Window-Mod类型启用
    /// </summary>
    public string TacticPlayingModitemEnable { get; }

    /// <summary>
    ///     战术播放Window-Mod类型启用-未分类
    /// </summary>
    public string TacticPlayingModitemNone { get; }

    /// <summary>
    ///     战术播放Window-Mod类型启用-生产单位
    /// </summary>
    public string TacticPlayingModitemWorker { get; }

    /// <summary>
    ///     战术播放Window-Mod类型启用-军事单位
    /// </summary>
    public string TacticPlayingModitemArmy { get; }

    /// <summary>
    ///     战术播放Window-Mod类型启用-建筑
    /// </summary>
    public string TacticPlayingModitemBuilding { get; }

    /// <summary>
    ///     战术播放Window-Mod类型启用-科技
    /// </summary>
    public string TacticPlayingModitemTech { get; }

    #endregion

    #region 设置页Page相关

    /// <summary>
    ///     设置页-GroupHeader-通用设置
    /// </summary>
    public string SettingsPageHeaderNormal { get; }

    /// <summary>
    ///     设置页-GroupHeader-快捷键设置
    /// </summary>
    public string SettingsPageHeaderHotkey { get; }

    /// <summary>
    ///     设置页-GroupHeader-关于
    /// </summary>
    public string SettingsPageHeaderAbout { get; }

    /// <summary>
    ///     设置页-通用设置-检查更新
    /// </summary>
    public string SettingsPageNormalUpgrade { get; }

    /// <summary>
    ///     设置页-关于-额外说明
    /// </summary>
    public string SettingsPageAboutRights { get; }

    #endregion

    #region 编辑器Page相关

    /// <summary>
    ///     编辑器Page-MOD内容索引
    /// </summary>
    public string EditorHeaderModItems { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-文件
    /// </summary>
    public string EditorMenuHeaderFiles { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-文件-新建文件
    /// </summary>
    public string EditorMenuFilesNew { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-文件-打开文件
    /// </summary>
    public string EditorMenuFilesOpen { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-文件-保存文件
    /// </summary>
    public string EditorMenuFilesSave { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-文件-另存为文件
    /// </summary>
    public string EditorMenuFilesSaveAs { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-文件-导出战术文件
    /// </summary>
    public string EditorMenuFilesExport { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-文件-导出战术文件至
    /// </summary>
    public string EditorMenuFilesExportTo { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-编辑
    /// </summary>
    public string EditorMenuHeaderEdit { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-编辑-撤销
    /// </summary>
    public string EditorMenuEditUndo { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-编辑-重做
    /// </summary>
    public string EditorMenuEditRedo { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-编辑-插入模板
    /// </summary>
    public string EditorMenuEditInsertTemplate { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-帮助
    /// </summary>
    public string EditorMenuHeaderHelp { get; }

    /// <summary>
    ///     编辑器Page-菜单栏-帮助-获取帮助
    /// </summary>
    public string EditorMenuHelpGetHelp { get; }

    /// <summary>
    ///     编辑器Page-错误信息-文件无法转换为战术文件
    /// </summary>
    public string EditorErrorFileCantConvert { get; }

    /// <summary>
    ///     编辑器Page-错误信息-Mod未设置无法启动关键词补全
    /// </summary>
    public string EditorErrorModNotSet { get; }

    /// <summary>
    ///     编辑器Page-导出成功信息标题
    /// </summary>
    public string EditorExportSuccessTitle { get; }

    /// <summary>
    ///     编辑器Page-导出成功信息内容
    /// </summary>
    public string EditorExportSuccessInfo { get; }

    #endregion

    #region Replay分析Page相关

    /// <summary>
    ///     Replay分析Page-功能开发中
    /// </summary>
    public string ReplayAnalysisDeveloping { get; }

    /// <summary>
    ///     Replay分析Page-按钮-解析SC2Replay
    /// </summary>
    public string ReplayAnalysisBtnDecodeSc2Replay { get; }

    /// <summary>
    ///     Replay分析Page-解析错误
    /// </summary>
    public string ReplayAnalysisDecodeError { get; }

    /// <summary>
    ///     Replay分析Page-解析错误标题
    /// </summary>
    public string ReplayAnalysisDecodeErrorTitle { get; }

    /// <summary>
    ///     Replay分析Page-解析成功
    /// </summary>
    public string ReplayAnalysisDecodeSuccess { get; }

    /// <summary>
    ///     Replay分析Page-解析成功标题
    /// </summary>
    public string ReplayAnalysisDecodeSuccessTitle { get; }

    #endregion

    #endregion

    #region 错误码部分

    /// <summary>
    ///     异常信息的文本模板
    /// </summary>
    public string ErrorDescTemplate { get; }

    /// <summary>
    ///     重复启动进程
    /// </summary>
    public string ErrorMuiltProcess { get; }

    #endregion

    #region 通用部分

    /// <summary>
    ///     通用气泡错误提示
    /// </summary>
    public string ToastTitleError { get; }

    /// <summary>
    ///     通用按钮文本-返回
    /// </summary>
    public string ButtonTxtBack { get; }

    /// <summary>
    ///     通用按钮文本-确定
    /// </summary>
    public string ButtonTxtSubmit { get; }

    /// <summary>
    ///     通用按钮文本-刷新
    /// </summary>
    public string ButtonTxtRefresh { get; }

    /// <summary>
    ///     通用文本—可用
    /// </summary>
    public string NormalTextAvailable { get; }

    /// <summary>
    ///     通用文本—错误
    /// </summary>
    public string NormalTextError { get; }

    /// <summary>
    ///     通用文本-功能开发中，敬请期待
    /// </summary>
    public string NormalTextDeveloping { get; }

    /// <summary>
    ///     友情链接
    /// </summary>
    public string NormalTextFriendshipLinks { get; }

    #endregion
}