using Avalonia.Input;
using TactiX_Models.Tactics;

namespace TactiX_Models;

/// <summary>
///     程序整体配置文件
/// </summary>
[Serializable]
public class LConfig
{
     /// <summary>
     ///     最终用户许可协议接受
     /// </summary>
     public bool EulaAccepted { get; set; } = false;

     /// <summary>
     ///     隐私条款接受
     /// </summary>
     public bool PpAccepted { get; set; } = false;

     /// <summary>
     ///     是否是夜间模式
     /// </summary>
     public bool NightMode { get; set; } = false;

     /// <summary>
     ///     当前启用的MOD的路径
     /// </summary>
     public string CurrentlyEnabledMod { get; set; } = string.Empty;

     /// <summary>
     ///     战术播放模式下的透明度
     /// </summary>
     public double Opacity { get; set; } = 0.7;

     /// <summary>
     ///     启用时间轴矫正
     /// </summary>
     public bool EnableTlCorr { get; set; } = true;

     /// <summary>
     ///     热键绑定，默认使用Alt+方向键
     /// </summary>
     public LHotkeyBinding[] Hotkeys { get; set; } =
    [
        new()
        {
            Hotkey = LHotkeyBindingEnum.StartOrResume,
            Modifiers = KeyModifiers.Alt,
            Key = Key.Up
        },
        new()
        {
            Hotkey = LHotkeyBindingEnum.Stop,
            Modifiers = KeyModifiers.Alt,
            Key = Key.Down
        },
        new()
        {
            Hotkey = LHotkeyBindingEnum.Previous,
            Modifiers = KeyModifiers.Alt,
            Key = Key.Left
        },
        new()
        {
            Hotkey = LHotkeyBindingEnum.Next,
            Modifiers = KeyModifiers.Alt,
            Key = Key.Right
        }
    ];

     /// <summary>
     ///     Mod中元素的播放设定
     /// </summary>
     public Dictionary<LModItemTypeEnum, bool> ModItemTypeEnable { get; set; } =
        new()
        {
            { LModItemTypeEnum.None, true },
            { LModItemTypeEnum.Worker, true },
            { LModItemTypeEnum.Army, true },
            { LModItemTypeEnum.Building, true },
            { LModItemTypeEnum.Tech, true }
        };
}