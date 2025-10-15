namespace TactiX_Models
{
    /// <summary>
    /// 程序整体配置文件
    /// </summary>
    [Serializable]
    public class L_Config
    {
        /// <summary>
        /// 最终用户许可协议接受
        /// </summary>
        public bool EulaAccepted { get; set; } = false;
        /// <summary>
        /// 隐私条款接受
        /// </summary>
        public bool PPAccepted { get; set; } = false;
        /// <summary>
        /// 是否是夜间模式
        /// </summary>
        public bool NightMode { get; set; } = false;
        /// <summary>
        /// 当前启用的MOD的路径
        /// </summary>
        public string CurrentlyEnabledMOD { get; set; } = "None.";
    }
}
