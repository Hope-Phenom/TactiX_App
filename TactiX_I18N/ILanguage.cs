namespace TactiX_I18N
{
    /// <summary>
    /// 语言接口
    /// </summary>
    public interface ILanguage
    {
        #region UI部分文本

        #region 错误信息弹窗
        public string ERROR_POPUP_TITLE { get; }
        public string ERROR_POPUP_LABEL_ERROR_CODE { get; }
        public string ERROR_POPUP_LABEL_ERROR_DESC { get; }
        public string ERROR_POPUP_LABEL_FEEDBACK { get; }
        public string ERROR_POPUP_LABEL_USER_DESC { get; }
        public string ERROR_POPUP_TBX_USER_DESC { get; }
        public string ERROR_POPUP_BTN_SEND_FEEDBACK { get; }
        public string ERROR_POPUP_BTN_CLOSE { get; }
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
    }
}
