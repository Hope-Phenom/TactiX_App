namespace TactiX_I18N
{
    /// <summary>
    /// 语言接口
    /// </summary>
    public interface ILanguage
    {
        #region UI部分文本

        #region 许可协议弹窗
        /// <summary>
        /// 许可协议弹窗-按钮-接受
        /// </summary>
        public string LICENSE_VIEW_BTN_ACCEPT { get; }
        /// <summary>
        /// 许可协议弹窗-按钮-拒绝
        /// </summary>
        public string LICENSE_VIEW_BTN_DECLINE { get; }
        #endregion

        #region 版本控制提示信息
        /// <summary>
        /// 版本控制提示信息-Toast信息标题
        /// </summary>
        public string VERSION_CONTROL_TOAST_TITLE { get; }
        /// <summary>
        /// 版本控制提示信息-当前版本已被禁用
        /// </summary>
        public string VERSION_CONTROL_BANNED { get; }
        /// <summary>
        /// 版本控制提示信息-有强制更新
        /// </summary>
        public string VERSION_CONTROL_FORCE_UPGRADE { get; }
        /// <summary>
        /// 版本控制提示信息-有新版本，请考虑更新
        /// </summary>
        public string VERSION_CONTROL_NEW_VERSION { get; }
        /// <summary>
        /// 版本控制提示信息-检查版本信息时发生错误
        /// </summary>
        public string VERSION_CONTROL_ERROR { get; }
        #endregion

        #region 错误信息弹窗
        /// <summary>
        /// 错误信息弹窗-标题
        /// </summary>
        public string ERROR_POPUP_TITLE { get; }
        /// <summary>
        /// 错误信息弹窗-错误码
        /// </summary>
        public string ERROR_POPUP_LABEL_ERROR_CODE { get; }
        /// <summary>
        /// 错误信息弹窗-错误信息
        /// </summary>
        public string ERROR_POPUP_LABEL_ERROR_DESC { get; }
        /// <summary>
        /// 错误信息弹窗-用户反馈渠道
        /// </summary>
        public string ERROR_POPUP_LABEL_FEEDBACK { get; }
        /// <summary>
        /// 错误信息弹窗-用户反馈的信息
        /// </summary>
        public string ERROR_POPUP_LABEL_USER_DESC { get; }
        /// <summary>
        /// 错误信息弹窗-用户反馈信息文本框水纹
        /// </summary>
        public string ERROR_POPUP_TBX_USER_DESC { get; }
        /// <summary>
        /// 错误信息弹窗-按钮-发送反馈
        /// </summary>
        public string ERROR_POPUP_BTN_SEND_FEEDBACK { get; }
        /// <summary>
        /// 错误信息弹窗-按钮-关闭
        /// </summary>
        public string ERROR_POPUP_BTN_CLOSE { get; }
        #endregion

        #region 主页面
        /// <summary>
        /// 主页面-侧栏按钮-新闻页面
        /// </summary>
        public string HOMESCREEN_SIDE_NEWS { get; }
        /// <summary>
        /// 主页面-侧栏按钮-战术大厅
        /// </summary>
        public string HOMESCREEN_SIDE_TACTICSHALL { get; }
        /// <summary>
        /// 主页面-侧栏按钮-MOD管理
        /// </summary>
        public string HOMESCREEN_SIDE_MOD_MANANGMENT { get; }
        #endregion

        #region 新闻Page相关
        /// <summary>
        /// 新闻Page-标题-论坛热帖
        /// </summary>
        public string NEWS_PAGE_VIEW_TITLE_FORUM { get; }
        /// <summary>
        /// 新闻Page-标题-系统新闻
        /// </summary>
        public string NEWS_PAGE_VIEW_TITLE_SYS_NEWS { get; }
        /// <summary>
        /// 新闻Page-标题-视频
        /// </summary>
        public string NEWS_PAGE_VIEW_TITLE_VIDEOS { get; }
        /// <summary>
        /// 新闻Page-错误信息-网络连接
        /// </summary>
        public string NEWS_PAGE_ERROR_NETWORK { get; }
        #endregion

        #region 战术大厅Page相关

        #endregion

        #region MOD管理Page相关
        /// <summary>
        /// MOD管理Page-本地MOD列表-HEADER
        /// </summary>
        public string MODS_MANAGE_VIEW_LOCAL_MODS_HEADER { get; }
        /// <summary>
        /// MOD管理Page-打开本地目录
        /// </summary>
        public string MODS_MANAGE_VIEW_LOCAL_MODS_FOLDER { get; }
        /// <summary>
        /// MOD管理Page-本地MOD信息
        /// </summary>
        public string MODS_MANAGE_VIEW_LOCAL_MODS_INFO { get; }
        /// <summary>
        /// MOD管理Page-选定MOD信息-HEADER
        /// </summary>
        public string MODS_MANAGE_VIEW_SELECTED_MOD_INFO { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作
        /// </summary>
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ACTION { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-装载错误
        /// </summary>
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ERROR { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-已启用MOD展示
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_LABEL_SELECTED { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-启用选定MOD
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_ENABLE_SELECTED { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-删除选定MOD
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-删除选定MOD错误
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED_ERROR { get; }
        #endregion

        #region 战术播放Window相关
        /// <summary>
        /// 战术播放Window-没有选定MOD文件
        /// </summary>
        public string TACTIC_PLAYING_MOD_NOT_SELECTED { get; }
        /// <summary>
        /// 战术播放Window-MOD文件不存在
        /// </summary>
        public string TACTIC_PLAYING_MOD_NOT_EXISTS { get; }
        /// <summary>
        /// 战术播放Window-MOD文件格式不正确
        /// </summary>
        public string TACTIC_PLAYING_MOD_FORMAT_ERROR { get; }
        #endregion

        #endregion

        #region 错误码部分

        /// <summary>
        /// 异常信息的文本模板
        /// </summary>
        public string ERROR_DESC_TEMPLATE { get; }
        /// <summary>
        /// 重复启动进程
        /// </summary>
        public string ERROR_MUILT_PROCESS { get; }

        #endregion

        #region 通用部分
        /// <summary>
        /// 通用气泡错误提示
        /// </summary>
        public string TOAST_TITLE_ERROR { get; }
        /// <summary>
        /// 通用按钮文本-返回
        /// </summary>
        public string BUTTON_TXT_BACK { get; }
        /// <summary>
        /// 通用按钮文本-确定
        /// </summary>
        public string BUTTON_TXT_SUBMIT { get; }
        /// <summary>
        /// 通用按钮文本-刷新
        /// </summary>
        public string BUTTON_TXT_REFRESH { get; }
        #endregion
    }
}
