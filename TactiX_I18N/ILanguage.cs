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
        public string TOAST_TITLE_ERROR { get; }
        #endregion
    }
}
