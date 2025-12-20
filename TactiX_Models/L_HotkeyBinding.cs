using Avalonia.Input;

namespace TactiX_Models;

/// <summary>
///     热键绑定
/// </summary>
[Serializable]
public class L_HotkeyBinding
{
    public L_HotkeyBindingEnum Hotkey { get; set; }
    public Key Key { get; set; }
    public KeyModifiers Modifiers { get; set; }
}

/// <summary>
///     热键类型枚举
/// </summary>
public enum L_HotkeyBindingEnum
{
    /// <summary>
    ///     开始或继续
    /// </summary>
    StartOrResume,

    /// <summary>
    ///     停止
    /// </summary>
    Stop,

    /// <summary>
    ///     上一步
    /// </summary>
    Previous,

    /// <summary>
    ///     下一步
    /// </summary>
    Next
}