namespace TactiX_Models.MessageBus;

public class MB_Hotkey
{
    public MB_Hotkey()
    {
    }

    public MB_Hotkey(L_HotkeyBindingEnum hotkeyEnum)
    {
        HotkeyEnum = hotkeyEnum;
    }

    public L_HotkeyBindingEnum HotkeyEnum { get; set; }
}