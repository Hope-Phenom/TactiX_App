using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Tactics
{
    /// <summary>
    /// 战术步骤，使用ID和MOD项进行关联
    /// </summary>
    [Serializable]
    public class L_TacticAction
    {
        /// <summary>
        /// 步骤步数
        /// </summary>
        public ushort Step { get; set; } = 0;
        /// <summary>
        /// 步骤对应MOD单位的缩写
        /// </summary>
        public string ItemAbbr { get; set; } = string.Empty;
        /// <summary>
        /// 时间/秒
        /// </summary>
        public uint Time { get; set; } = 0;

        /// <summary>
        /// 动作对象，只记录基础信息和MOD中的映射关系
        /// </summary>
        public L_TacticAction() { }
        /// <summary>
        /// 动作对象，只记录基础信息和MOD中的映射关系
        /// </summary>
        /// <param name="stepNo"></param>
        /// <param name="item"></param>
        /// <param name="time"></param>
        public L_TacticAction(ushort stepNo, string itemAbbr, uint time)
        {
            Step = stepNo;
            ItemAbbr = itemAbbr;
            Time = time;
        }
    }
}
