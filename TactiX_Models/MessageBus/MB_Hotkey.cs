using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    public class MB_Hotkey
    {
        public L_HotkeyBindingEnum HotkeyEnum { get; set; }

        public MB_Hotkey() { }

        public MB_Hotkey(L_HotkeyBindingEnum hotkeyEnum)
        {
            HotkeyEnum = hotkeyEnum;
        }
    }
}
