using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_Models.MessageBus
{
    /// <summary>
    /// 导航信息，用于通知MainView跳转页面
    /// </summary>
    public class MB_NavigationTo
    {
        public required Type NaviType { get; set; }
    }
}
