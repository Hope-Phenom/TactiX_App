using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_OS_Tools
{
    public class OSTools
    {
        #region 单例模式
        private static readonly Lazy<OSTools> _lazyInstance = new Lazy<OSTools>(() => new OSTools(), isThreadSafe: true);
        public static OSTools Instance => _lazyInstance.Value;
        private OSTools() { }
        #endregion
    }
}
