using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    public class MB_ToastVersion : MB_ToastPureText
    {
        /// <summary>
        /// 版本发布地址，用于网页跳转
        /// </summary>
        public required string Release_Url { get; set; }
    }
}
