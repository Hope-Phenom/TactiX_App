using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Tactics
{
    /// <summary>
    /// Mod对象
    /// </summary>
    [Serializable]
    public class L_ModItem
    {
        /// <summary>
        /// 用于编辑时的缩写
        /// </summary>
        public string Abbr { get; set; } = string.Empty;
        /// <summary>
        /// 展示文本
        /// </summary>
        public string Desc { get; set; } = string.Empty;
    }
}
