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
        public int Gameloop { get; set; }
        public int Time { get; set; }
        public required string UnitName { get; set; }

        public override string ToString()
        {
            return $"GameLoop: {Gameloop}, UnitName: {UnitName}, Time: {Time}";
        }
    }
}
