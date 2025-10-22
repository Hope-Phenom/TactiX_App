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
        public string BUTTON_TXT_BACK => "返回";
        public string BUTTON_TXT_SUBMIT => "确定";
        public string BUTTON_TXT_REFRESH => "刷新";
        public string NORMAL_TEXT_AVAILABLE => "可用";
        public string NORMAL_TEXT_ERROR => "错误";

        public string HOMESCREEN_SIDE_NEWS => "新闻资讯";
        public string HOMESCREEN_SIDE_TACTICSHALL => "战术大厅";
        public string HOMESCREEN_SIDE_MOD_MANANGMENT => "MOD管理";
        public string HOMESCREEN_SIDE_SETTINGS => "设置";
        public string HOMESCREEN_SIDE_EDITOR => "战术编辑";

        public string NEWS_PAGE_VIEW_TITLE_FORUM => "论坛热帖";
        public string NEWS_PAGE_VIEW_TITLE_SYS_NEWS => "系统公告";
        public string NEWS_PAGE_VIEW_TITLE_VIDEOS => "视频推荐";
        public string NEWS_PAGE_ERROR_NETWORK => "由于网络问题，未能更新新闻信息";

        public string MODS_MANAGE_VIEW_LOCAL_MODS_HEADER => "本地Mod包";
        public string MODS_MANAGE_VIEW_LOCAL_MODS_FOLDER => "打开本地目录";
        public string MODS_MANAGE_VIEW_LOCAL_MODS_INFO => "本地检索到[{0}]个可用Mod(s)。";
        public string MODS_MANAGE_VIEW_SELECTED_MOD_INFO => "选定MOD信息";
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ACTION => "MOD管理";
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ERROR => "尝试读取[{0}]Mod包时发生错误，错误信息：{1}";
        public string MODS_MANAGE_VIEW_MOD_ACTION_LABEL_SELECTED => "当前启用MOD：";
        public string MODS_MANAGE_VIEW_MOD_ACTION_ENABLE_SELECTED => "启用选定MOD";
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED => "删除选定MOD";
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED_ERROR => "尝试删除[{0}]Mod包时发生错误，错误信息：{1}";

        public string TACTIC_PLAYING_MOD_NOT_SELECTED => "没有选定MOD文件，无法打开战术播放界面。请前往MOD管理选项卡进行配置。";
        public string TACTIC_PLAYING_MOD_NOT_EXISTS => "配置的MOD文件不存在，请前往MOD管理选项卡进行检查。";
        public string TACTIC_PLAYING_MOD_FORMAT_ERROR => "尝试加载的MOD文件格式不正确，请前往MOD管理选项卡进行检查。";
        public string TACTIC_PLAYING_LABEL_FILE_SELECTED => "选择战术文件：";
        public string TACTIC_PLAYING_LABEL_CURR_REAL_TIMESTAMP => "运行时间：";
        public string TACTIC_PLAYING_LABEL_CURR_STEP_TIMESTAMP => "战术时间：";
        public string TACTIC_PLAYING_HOTKEY_ALREADY_EXSITS => "热键[{0}+{1}]已被占用，请检查是否与其他程序存在冲突！";
        public string TACTIC_PLAYING_OPACITY => "战术播放透明度：";
        public string TACTIC_PLAYING_TIMELINE_CORRECTION => "启用时间轴矫正";
        public string TACTIC_PLAYING_FILEINFO => "战术文件介绍：";

        public string SETTINGS_PAGE_HEADER_NORMAL => "通用设置";
        public string SETTINGS_PAGE_HEADER_HOTKEY => "快捷键设置";
        public string SETTINGS_PAGE_HEADER_ABOUT => "关于";
        public string SETTINGS_PAGE_NORMAL_UPGRADE => "检查更新";

        public string EDITOR_HEADER_MOD_ITEMS => "MOD内容索引";
        public string EDITOR_MENU_HEADER_FILES => "文件";
        public string EDITOR_MENU_FILES_NEW => "创建新的战术文件";
        public string EDITOR_MENU_FILES_OPEN => "打开已有战术文件";
        public string EDITOR_MENU_FILES_SAVE => "保存当前战术文件";
        public string EDITOR_MENU_FILES_SAVE_AS => "另存为当前战术文件";
        public string EDITOR_MENU_FILES_EXPORT => "导出为战术文件(*.tactix)";
        public string EDITOR_MENU_HEADER_EDIT => "编辑";
        public string EDITOR_MENU_EDIT_UNDO => "撤销";
        public string EDITOR_MENU_EDIT_REDO => "重做";
        public string EDITOR_MENU_EDIT_INSERT_TEMPLATE => "插入模板";
        public string EDITOR_MENU_HEADER_HELP => "帮助";
        public string EDITOR_MENU_HELP_GET_HELP => "获取关于战术编辑的帮助";
        public string EDITOR_ERROR_FILE_CANT_CONVERT => "无法导出战术文件，请检查格式是否符合要求";
    }
}
