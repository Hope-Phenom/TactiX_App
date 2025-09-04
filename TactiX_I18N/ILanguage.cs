namespace TactiX_I18N
{
    /// <summary>
    /// 语言接口
    /// </summary>
    public interface ILanguage
    {
        #region UI部分文本
        #endregion

        #region 错误码部分

        /// <summary>
        /// 重复启动进程
        /// </summary>
        public string ERROR_MUILT_PROCESS { get; }

        #endregion
    }
}
