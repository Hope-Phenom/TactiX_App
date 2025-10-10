using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Tactics
{
    /// <summary>
    /// 战术文件分类
    /// </summary>
    public enum L_TacticEnum
    {
        /// <summary>
        /// 时间轴模式，按时间点自动播放，允许手动暂停/继续和上/下步
        /// </summary>
        TIMELINE,
        /// <summary>
        /// 单步模式，仅支持手动上/下步
        /// </summary>
        STEP
    }
}
