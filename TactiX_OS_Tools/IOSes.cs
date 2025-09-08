namespace TactiX_OS_Tools
{
    /// <summary>
    /// 不同OS的SDK功能接口
    /// </summary>
    public interface IOSes
    {
        /// <summary>
        /// 用户数据的存放路径
        /// </summary>
        public string UserDataPath { get; }
        /// <summary>
        /// 是否是启动的唯一副本（仅针对移动端以外生效）
        /// </summary>
        public bool IsSingleton { get; }
        /// <summary>
        /// 设置窗口鼠标穿透（仅针对移动端以外生效）
        /// </summary>
        /// <param name="hwnd">句柄</param>
        public void SetMouseTransport(IntPtr hwnd);
    }
}
