using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactiX_I18N
{
    public class Chinese : ILanguage
    {
        public string ERROR_POPUP_TITLE => "错误信息";
        public string ERROR_POPUP_LABEL_ERROR_CODE => "错误代码：";
        public string ERROR_POPUP_LABEL_ERROR_DESC => "错误信息：";
        public string ERROR_POPUP_LABEL_FEEDBACK => "反馈途径（Email、QQ等）：";
        public string ERROR_POPUP_LABEL_USER_DESC => "错误情形描述：";
        public string ERROR_POPUP_TBX_USER_DESC => "请在此文本框中输入错误情形。";
        public string ERROR_POPUP_BTN_SEND_FEEDBACK => "发送错误报告";
        public string ERROR_POPUP_BTN_CLOSE => "关闭";

        public string ERROR_DESC_TEMPLATE => "程序发生异常，错误码：{0}，错误信息：{1}。";
        public string ERROR_MUILT_PROCESS => "重复打开了多个进程，本进程将退出！";

    }
}
