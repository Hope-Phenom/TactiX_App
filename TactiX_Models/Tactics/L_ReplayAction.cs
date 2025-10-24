using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Tactics
{
    /// <summary>
    /// Replay解析步骤的数据结构体
    /// </summary>
    [Serializable]
    public class L_ReplayAction
    {
        /// <summary>
        /// 游戏循环数
        /// </summary>
        public int Gameloop { get; set; }
        /// <summary>
        /// 游戏时间
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public required string UnitName { get; set; }
        /// <summary>
        /// 单位缩写
        /// </summary>
        public required string Abbr { get; set; }

        public override string ToString()
        {
            return $"GameLoop: {Gameloop}, UnitName: [{UnitName}/{Abbr}], Time: {Time}";
        }
    }
}
