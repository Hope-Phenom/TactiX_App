using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Tactics
{
    /// <summary>
    /// Mod文件的描述类，包括基本信息和动作、单位映射关系
    /// </summary>
    [Serializable]
    public class L_ModDesc
    {
        /// <summary>
        /// Mod名称
        /// </summary>
        public string ModName { get; set; } = string.Empty;
        /// <summary>
        /// Mod版本号
        /// </summary>
        public ulong ModVersion { get; set; } = ulong.MinValue;
        /// <summary>
        /// 动作的映射关系
        /// </summary>
        public List<L_ModItem> Actions { get; set; } = new();
        /// <summary>
        /// 单位的映射关系
        /// </summary>
        public List<L_ModItem> Units { get; set; } = new();
        /// <summary>
        /// 作者，展示用
        /// </summary>
        public string Author { get; set; } = string.Empty;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
