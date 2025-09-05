using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tactix_Exception
{
    public enum TactiXErrorCodes
    {
        /// <summary>
        /// 成功，无异常
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// 重复启动进程
        /// </summary>
        ERROR_MUILT_PROCESS,
    }
}
