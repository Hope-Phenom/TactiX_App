using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    /// <summary>
    /// 通知消息，通知对应的Window修改标题文本
    /// </summary>
    public class MB_WindowTitle
    {
        public required string Title { get; set; }
        public required string WindowName { get; set; }
    }
}
