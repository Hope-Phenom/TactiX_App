namespace TactiX_I18N
{
    /// <summary>
    /// 语言接口
    /// </summary>
    public interface ILanguage
    {
        #region UI部分文本

        #region 许可协议弹窗
        /// <summary>
        /// 许可协议弹窗-按钮-接受
        /// </summary>
        public string LICENSE_VIEW_BTN_ACCEPT { get; }
        /// <summary>
        /// 许可协议弹窗-按钮-拒绝
        /// </summary>
        public string LICENSE_VIEW_BTN_DECLINE { get; }
        #endregion

        #region 版本控制提示信息
        /// <summary>
        /// 版本控制提示信息-Toast信息标题
        /// </summary>
        public string VERSION_CONTROL_TOAST_TITLE { get; }
        /// <summary>
        /// 版本控制提示信息-当前版本已被禁用
        /// </summary>
        public string VERSION_CONTROL_BANNED { get; }
        /// <summary>
        /// 版本控制提示信息-有强制更新
        /// </summary>
        public string VERSION_CONTROL_FORCE_UPGRADE { get; }
        /// <summary>
        /// 版本控制提示信息-有新版本，请考虑更新
        /// </summary>
        public string VERSION_CONTROL_NEW_VERSION { get; }
        /// <summary>
        /// 版本控制提示信息-检查版本信息时发生错误
        /// </summary>
        public string VERSION_CONTROL_ERROR { get; }
        #endregion

        #region 错误信息弹窗
        /// <summary>
        /// 错误信息弹窗-标题
        /// </summary>
        public string ERROR_POPUP_TITLE { get; }
        /// <summary>
        /// 错误信息弹窗-错误码
        /// </summary>
        public string ERROR_POPUP_LABEL_ERROR_CODE { get; }
        /// <summary>
        /// 错误信息弹窗-错误信息
        /// </summary>
        public string ERROR_POPUP_LABEL_ERROR_DESC { get; }
        /// <summary>
        /// 错误信息弹窗-用户反馈渠道
        /// </summary>
        public string ERROR_POPUP_LABEL_FEEDBACK { get; }
        /// <summary>
        /// 错误信息弹窗-用户反馈的信息
        /// </summary>
        public string ERROR_POPUP_LABEL_USER_DESC { get; }
        /// <summary>
        /// 错误信息弹窗-用户反馈信息文本框水纹
        /// </summary>
        public string ERROR_POPUP_TBX_USER_DESC { get; }
        /// <summary>
        /// 错误信息弹窗-按钮-发送反馈
        /// </summary>
        public string ERROR_POPUP_BTN_SEND_FEEDBACK { get; }
        /// <summary>
        /// 错误信息弹窗-按钮-关闭
        /// </summary>
        public string ERROR_POPUP_BTN_CLOSE { get; }
        #endregion

        #region 主页面
        /// <summary>
        /// 主页面-侧栏按钮-新闻页面
        /// </summary>
        public string HOMESCREEN_SIDE_NEWS { get; }
        /// <summary>
        /// 主页面-侧栏按钮-战术大厅
        /// </summary>
        public string HOMESCREEN_SIDE_TACTICSHALL { get; }
        /// <summary>
        /// 主页面-侧栏按钮-MOD管理
        /// </summary>
        public string HOMESCREEN_SIDE_MOD_MANANGMENT { get; }
        /// <summary>
        /// 主页面-侧栏按钮-设置
        /// </summary>
        public string HOMESCREEN_SIDE_SETTINGS { get; }
        /// <summary>
        /// 主页面-侧边按钮-战术编辑
        /// </summary>
        public string HOMESCREEN_SIDE_EDITOR { get; }
        #endregion

        #region 新闻Page相关
        /// <summary>
        /// 新闻Page-标题-论坛热帖
        /// </summary>
        public string NEWS_PAGE_VIEW_TITLE_FORUM { get; }
        /// <summary>
        /// 新闻Page-标题-系统新闻
        /// </summary>
        public string NEWS_PAGE_VIEW_TITLE_SYS_NEWS { get; }
        /// <summary>
        /// 新闻Page-标题-视频
        /// </summary>
        public string NEWS_PAGE_VIEW_TITLE_VIDEOS { get; }
        /// <summary>
        /// 新闻Page-错误信息-网络连接
        /// </summary>
        public string NEWS_PAGE_ERROR_NETWORK { get; }
        #endregion

        #region 战术大厅Page相关

        #endregion

        #region MOD管理Page相关
        /// <summary>
        /// MOD管理Page-本地MOD列表-HEADER
        /// </summary>
        public string MODS_MANAGE_VIEW_LOCAL_MODS_HEADER { get; }
        /// <summary>
        /// MOD管理Page-打开本地目录
        /// </summary>
        public string MODS_MANAGE_VIEW_LOCAL_MODS_FOLDER { get; }
        /// <summary>
        /// MOD管理Page-本地MOD信息
        /// </summary>
        public string MODS_MANAGE_VIEW_LOCAL_MODS_INFO { get; }
        /// <summary>
        /// MOD管理Page-选定MOD信息-HEADER
        /// </summary>
        public string MODS_MANAGE_VIEW_SELECTED_MOD_INFO { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作
        /// </summary>
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ACTION { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-装载错误
        /// </summary>
        public string MODS_MANAGE_VIEW_SELECTED_MOD_ERROR { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-已启用MOD展示
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_LABEL_SELECTED { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-启用选定MOD
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_ENABLE_SELECTED { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-删除选定MOD
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED { get; }
        /// <summary>
        /// MOD管理Page-选定MOD操作-删除选定MOD错误
        /// </summary>
        public string MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED_ERROR { get; }
        #endregion

        #region 战术播放Window相关
        /// <summary>
        /// 战术播放Window-没有选定MOD文件
        /// </summary>
        public string TACTIC_PLAYING_MOD_NOT_SELECTED { get; }
        /// <summary>
        /// 战术播放Window-MOD文件不存在
        /// </summary>
        public string TACTIC_PLAYING_MOD_NOT_EXISTS { get; }
        /// <summary>
        /// 战术播放Window-MOD文件格式不正确
        /// </summary>
        public string TACTIC_PLAYING_MOD_FORMAT_ERROR { get; }
        /// <summary>
        /// 战术播放Window-选定战术文件
        /// </summary>
        public string TACTIC_PLAYING_LABEL_FILE_SELECTED { get; }
        /// <summary>
        /// 战术播放Window-当前的真实时间戳
        /// </summary>
        public string TACTIC_PLAYING_LABEL_CURR_REAL_TIMESTAMP { get; }
        /// <summary>
        /// 战术播放Window-当前战术步骤的时间戳
        /// </summary>
        public string TACTIC_PLAYING_LABEL_CURR_STEP_TIMESTAMP { get; }
        /// <summary>
        /// 战术播放Window-热键已被占用
        /// </summary>
        public string TACTIC_PLAYING_HOTKEY_ALREADY_EXSITS { get; }
        /// <summary>
        /// 战术播放Window-播放时透明度
        /// </summary>
        public string TACTIC_PLAYING_OPACITY { get; }
        /// <summary>
        /// 战术播放Window-启用时间轴矫正
        /// </summary>
        public string TACTIC_PLAYING_TIMELINE_CORRECTION { get; }
        /// <summary>
        /// 战术播放Window-战术文件介绍
        /// </summary>
        public string TACTIC_PLAYING_FILEINFO { get; }
        #endregion

        #region 设置页Page相关
        /// <summary>
        /// 设置页-GroupHeader-通用设置
        /// </summary>
        public string SETTINGS_PAGE_HEADER_NORMAL { get; }
        /// <summary>
        /// 设置页-GroupHeader-快捷键设置
        /// </summary>
        public string SETTINGS_PAGE_HEADER_HOTKEY { get; }
        /// <summary>
        /// 设置页-GroupHeader-关于
        /// </summary>
        public string SETTINGS_PAGE_HEADER_ABOUT { get; }
        /// <summary>
        /// 设置页-通用设置-检查更新
        /// </summary>
        public string SETTINGS_PAGE_NORMAL_UPGRADE { get; }
        #endregion

        #region 编辑器Page相关
        /// <summary>
        /// 编辑器Page-MOD内容索引
        /// </summary>
        public string EDITOR_HEADER_MOD_ITEMS { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-文件
        /// </summary>
        public string EDITOR_MENU_HEADER_FILES { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-文件-新建文件
        /// </summary>
        public string EDITOR_MENU_FILES_NEW { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-文件-打开文件
        /// </summary>
        public string EDITOR_MENU_FILES_OPEN { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-文件-保存文件
        /// </summary>
        public string EDITOR_MENU_FILES_SAVE { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-文件-另存为文件
        /// </summary>
        public string EDITOR_MENU_FILES_SAVE_AS { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-文件-导出为战术文件
        /// </summary>
        public string EDITOR_MENU_FILES_EXPORT { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-编辑
        /// </summary>
        public string EDITOR_MENU_HEADER_EDIT { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-编辑-撤销
        /// </summary>
        public string EDITOR_MENU_EDIT_UNDO { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-编辑-重做
        /// </summary>
        public string EDITOR_MENU_EDIT_REDO { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-编辑-插入模板
        /// </summary>
        public string EDITOR_MENU_EDIT_INSERT_TEMPLATE { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-帮助
        /// </summary>
        public string EDITOR_MENU_HEADER_HELP { get; }
        /// <summary>
        /// 编辑器Page-菜单栏-帮助-获取帮助
        /// </summary>
        public string EDITOR_MENU_HELP_GET_HELP { get; }
        /// <summary>
        /// 编辑器Page-错误信息-文件无法转换为战术文件
        /// </summary>
        public string EDITOR_ERROR_FILE_CANT_CONVERT { get; }

        #endregion

        #endregion

        #region 错误码部分

        /// <summary>
        /// 异常信息的文本模板
        /// </summary>
        public string ERROR_DESC_TEMPLATE { get; }
        /// <summary>
        /// 重复启动进程
        /// </summary>
        public string ERROR_MUILT_PROCESS { get; }

        #endregion

        #region 通用部分
        /// <summary>
        /// 通用气泡错误提示
        /// </summary>
        public string TOAST_TITLE_ERROR { get; }
        /// <summary>
        /// 通用按钮文本-返回
        /// </summary>
        public string BUTTON_TXT_BACK { get; }
        /// <summary>
        /// 通用按钮文本-确定
        /// </summary>
        public string BUTTON_TXT_SUBMIT { get; }
        /// <summary>
        /// 通用按钮文本-刷新
        /// </summary>
        public string BUTTON_TXT_REFRESH { get; }
        /// <summary>
        /// 通用文本—可用
        /// </summary>
        public string NORMAL_TEXT_AVAILABLE { get; }
        /// <summary>
        /// 通用文本—错误
        /// </summary>
        public string NORMAL_TEXT_ERROR { get; }
        #endregion
    }
}
