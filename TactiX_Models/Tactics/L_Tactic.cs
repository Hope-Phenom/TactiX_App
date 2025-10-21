using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Tactics
{
    /// <summary>
    /// 战术文件，包含基本信息、对应的MOD和步骤数据
    /// </summary>
    [Serializable]
    public class L_Tactic
    {
        /// <summary>
        /// 每个战术文件的唯一ID
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 战术文件名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 作者，展示用
        /// </summary>
        public string Author { get; set; } = string.Empty;
        /// <summary>
        /// 战术内容的介绍
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 适用版本
        /// </summary>
        public string ApplicableVersion {  get; set; } = string.Empty;

        /// <summary>
        /// 战术文件类型，时间线模式或者单步模式
        /// </summary>
        public L_TacticEnum TacticType { get; set; } = L_TacticEnum.TIMELINE;
        /// <summary>
        /// 战术文件的版本，相同Guid不同版本的文件会被视为同一个战术的迭代
        /// </summary>
        public ulong TacVersion { get; set; } = ulong.MinValue;
        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; } = string.Empty;

        /// <summary>
        /// 对应MOD的名称
        /// </summary>
        public string ModName { get; set; } = L_StaticValue.MOD_NAME_SCII;
        /// <summary>
        /// 对应MOD的最低版本，如果装载的MOD不符合版本要求应该进行提示
        /// </summary>
        public ulong ModVersion { get; set; } = ulong.MinValue;

        /// <summary>
        /// 动作列表
        /// </summary>
        public List<L_TacticAction> Actions { get; set; } = new();
    }
}
