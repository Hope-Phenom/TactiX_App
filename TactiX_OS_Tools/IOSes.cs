using TactiX_Models;

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
        public string AppDataFolderPath { get; }
        /// <summary>
        /// 获取配置文件
        /// </summary>
        public L_Config LoadConfig();
        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SaveConfig();
        /// <summary>
        /// 是否是启动的唯一副本（仅针对移动端以外生效）
        /// </summary>
        public bool IsSingleton { get; }
        /// <summary>
        /// 设置OSes功能实现时针对的窗口句柄
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        public void SetHandle(IntPtr hwnd);
        /// <summary>
        /// 切换窗口鼠标穿透（仅针对移动端以外生效）
        /// </summary>
        /// <param name="hwnd">句柄</param>
        public void SetMouseTransport();
        /// <summary>
        /// 跳转Web地址
        /// </summary>
        /// <param name="url">Web地址</param>
        public void OpenWeb(string url);
    }
}
