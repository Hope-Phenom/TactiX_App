using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    public class MB_ToastPureText
    {
        /// <summary>
        /// 提示类型
        /// </summary>
        public MB_Enum_ToastType Type { get; set; }
        /// <summary>
        /// 提示标题
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// 提示文本
        /// </summary>
        public required string Message { get; set; }
    }
}
