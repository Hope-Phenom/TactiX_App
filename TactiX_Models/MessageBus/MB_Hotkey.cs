namespace TactiX_Models.MessageBus;

public class MbHotkey
{
    public MbHotkey()
    {
    }

    public MbHotkey(LHotkeyBindingEnum hotkeyEnum)
    {
        HotkeyEnum = hotkeyEnum;
    }

    public LHotkeyBindingEnum HotkeyEnum { get; set; }
}