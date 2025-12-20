using Avalonia.Controls;
using Avalonia.Input;
using TactiX_Models;

namespace TactiX_OS_Tools;

/// <summary>
///     不同OS的SDK功能接口
/// </summary>
public interface IoSes
{
    /// <summary>
    ///     用户数据的存放路径
    /// </summary>
    public string AppDataFolderPath { get; }

    /// <summary>
    ///     是否是启动的唯一副本（仅针对移动端以外生效）
    /// </summary>
    public bool IsSingleton { get; }

    /// <summary>
    ///     获取配置文件
    /// </summary>
    public LConfig LoadConfig();

    /// <summary>
    ///     保存配置文件
    /// </summary>
    public void SaveConfig();

    /// <summary>
    ///     设置战术播放窗口的句柄
    /// </summary>
    public void SetTacticPlayingWindowHandle(IntPtr hwnd);

    /// <summary>
    ///     切换窗口鼠标穿透（仅针对移动端以外生效）
    /// </summary>
    /// <param name="hwnd">句柄</param>
    public void SetMouseTransport(bool enable);

    /// <summary>
    ///     跳转Web地址
    /// </summary>
    /// <param name="url">Web地址</param>
    public void OpenUrl(string url);

    /// <summary>
    ///     检查或创建指定的路径
    /// </summary>
    /// <param name="path">路径</param>
    public void CheckOrCreateDir(string path);

    /// <summary>
    ///     设置主窗体的句柄，用于后续创建全局快捷键（Win32）
    /// </summary>
    public void SetMainWindowHandle(IntPtr hwnd);

    /// <summary>
    ///     注册全局快捷键
    /// </summary>
    /// <param name="key">键值</param>
    /// <param name="modifiers">修饰键组合</param>
    /// <param name="action">触发时执行的操作</param>
    /// <returns>注册是否成功</returns>
    public bool RegisterHotkey(Key key, KeyModifiers modifiers, Action action);

    /// <summary>
    ///     注销所有热键
    /// </summary>
    public void UnregisterAllHotkeys();

    /// <summary>
    ///     检查热键是否可用
    /// </summary>
    public bool IsHotkeyAvailable(Key key, KeyModifiers modifiers);

    /// <summary>
    ///     注册WndProcHookCallback
    /// </summary>
    /// <param name="topLevel"></param>
    public void RegisterWndProcHookCallback(TopLevel topLevel);
}