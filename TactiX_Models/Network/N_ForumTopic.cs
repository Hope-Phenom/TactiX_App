using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Network
{
    /// <summary>
    /// 论坛热帖
    /// </summary>
    [Serializable]
    public class N_ForumTopic
    {
        /// <summary>
        /// 标题
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public required string Url { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public required string Date { get; set; }
    }
}
