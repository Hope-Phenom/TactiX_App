using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    public class MB_FileDialog
    {
        /// <summary>
        /// 是 → 文件打开窗口；否 → 文件保存窗口
        /// </summary>
        public bool IsOpenMode { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string? FilePath { get; set; }
        /// <summary>
        /// 请求的窗体名
        /// </summary>
        public required string WindowName { get; set; }
        /// <summary>
        /// 用于区分作用
        /// </summary>
        public required string Trigger { get; set; }
        /// <summary>
        /// 文件过滤器
        /// </summary>
        public string? FileFilter { get; set; }
        /// <summary>
        /// 文件过滤器名称
        /// </summary>
        public string? FileFilterName { get; set; }
        /// <summary>
        /// 建议初始目录
        /// </summary>
        public string? SuggestStartLocation { get; set; }
    }
}
