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
        public ushort StepNo { get; set; } = 0;
        /// <summary>
        /// 步骤对应MOD的序号
        /// </summary>
        public ushort Item { get; set; } = 0;
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
        public L_TacticAction(ushort stepNo, ushort item, uint time)
        {
            StepNo = stepNo;
            Item = item;
            Time = time;
        }
    }
}
