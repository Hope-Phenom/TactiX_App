namespace TactiX_Models
{
    /// <summary>
    /// 程序整体配置文件
    /// </summary>
    [Serializable]
    public class LConfig
    {
        /// <summary>
        /// 最终用户许可协议接受
        /// </summary>
        public bool EulaAccepted { get; set; } = false;
        /// <summary>
        /// 隐私条款接受
        /// </summary>
        public bool PPAccepted { get; set; } = false;
    }
}
