using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.Network
{
    [Serializable]
    public class N_VersionControlReq
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public required string Version { get; set; }
    }
}
