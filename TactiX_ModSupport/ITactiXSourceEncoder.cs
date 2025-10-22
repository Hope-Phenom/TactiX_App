using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_Models.Tactics;

namespace TactiX_ModSupport
{
    public interface ITactiXSourceEncoder
    {
        /// <summary>
        /// 解析传入的文本为战术文件
        /// </summary>
        public L_Tactic? Decoder(string[] lines);
    }
}
