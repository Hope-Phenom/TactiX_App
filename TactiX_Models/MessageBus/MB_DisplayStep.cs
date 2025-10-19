using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    public class MB_DisplayStep
    {
        /// <summary>
        /// 指定要进行展示的槽位序号
        /// </summary>
        public int SlotNo { get; set; }
        /// <summary>
        /// 展示的文本内容
        /// </summary>
        public string? Desc { get; set; }
        /// <summary>
        /// 展示的ICON，Bitmap格式
        /// </summary>
        public object? Image { get; set; }
    }
}
