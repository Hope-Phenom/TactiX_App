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
        public string DisplayText { get; set; } = string.Empty;
        /// <summary>
        /// 对应的图像文件的文件名
        /// </summary>
        public string ImageFile { get; set; } = string.Empty;
        /// <summary>
        /// 对应的声音文件的文件名
        /// </summary>
        public string VoiceFile { get; set; } = string.Empty;
    }
}
