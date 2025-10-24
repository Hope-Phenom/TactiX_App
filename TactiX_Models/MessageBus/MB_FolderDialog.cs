using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    public class MB_FolderDialog
    {
        /// <summary>
        /// 请求的窗体名
        /// </summary>
        public required string WindowName { get; set; }
        /// <summary>
        /// 用于区分作用
        /// </summary>
        public required string Trigger { get; set; }
        /// <summary>
        /// 目录地址
        /// </summary>
        public string? FolderPath { get; set; }
    }
}
