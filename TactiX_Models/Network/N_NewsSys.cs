using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Network
{
    /// <summary>
    /// 系统公告
    /// </summary>
    [Serializable]
    public class N_NewsSys
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; } = string.Empty;
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 日期二次封装
        /// </summary>
        public string DateTimeStr => DateTime.Now.ToString("yyyy-MM-dd");
    }
}
