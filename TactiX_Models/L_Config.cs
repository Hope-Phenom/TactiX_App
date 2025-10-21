using Avalonia.Collections;

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
        public string CurrentlyEnabledMOD { get; set; } = string.Empty;
        /// <summary>
        /// 战术播放模式下的透明度
        /// </summary>
        public double Opacity { get; set; } = 0.7;
        /// <summary>
        /// 启用时间轴矫正
        /// </summary>
        public bool EnableTLCorr { get; set; } = true;
        /// <summary>
        /// 热键绑定，默认使用Alt+方向键
        /// </summary>
        public L_HotkeyBinding[] Hotkeys =
        [
             new L_HotkeyBinding()
             {
                  Hotkey = L_HotkeyBindingEnum.StartOrResume,
                  Modifiers = Avalonia.Input.KeyModifiers.Alt,
                  Key = Avalonia.Input.Key.Up
             },
             new L_HotkeyBinding()
             {
                  Hotkey = L_HotkeyBindingEnum.Stop,
                  Modifiers = Avalonia.Input.KeyModifiers.Alt,
                  Key = Avalonia.Input.Key.Down
             },
             new L_HotkeyBinding()
             {
                  Hotkey = L_HotkeyBindingEnum.Previous,
                  Modifiers = Avalonia.Input.KeyModifiers.Alt,
                  Key = Avalonia.Input.Key.Left
             },
             new L_HotkeyBinding()
             {
                  Hotkey = L_HotkeyBindingEnum.Next,
                  Modifiers = Avalonia.Input.KeyModifiers.Alt,
                  Key = Avalonia.Input.Key.Right
             }
        ];
    }
}
