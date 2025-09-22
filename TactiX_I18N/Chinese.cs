namespace TactiX_I18N
{
    public class Chinese : ILanguage
    {
        public string LICENSE_VIEW_BTN_ACCEPT => "接受";
        public string LICENSE_VIEW_BTN_DECLINE => "拒绝";

        public string VERSION_CONTROL_TOAST_TITLE => "检查程序版本...";
        public string VERSION_CONTROL_BANNED => "当前版本[{0}]已被禁用，稍后程序将跳转至更新页面并自动退出！";
        public string VERSION_CONTROL_FORCE_UPGRADE => "存在强制更新版本[{0}]，稍后程序将跳转至更新页面并自动退出！";
        public string VERSION_CONTROL_NEW_VERSION => "当前版本[{0}]，存在新版本[{1}]，是否进行更新？";
        public string VERSION_CONTROL_ERROR => "检查版本信息时发生错误，错误信息：{0}";

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

        public string TOAST_TITLE_ERROR => "发生了一个错误";
    }
}
