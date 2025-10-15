using System;
using System.IO;

using Avalonia.Collections;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;

using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page
{
    public partial class ModsManagePageViewModel : ViewModelBase
    {
        #region DI注入
        public ILanguage Language { get; private set; }
        private readonly Logger _logger;
        private readonly IMessenger _messenger;
        private readonly IOSes _oses;
        #endregion

        #region 常量/变量
        private const string TACTICS_FOLDER = "Tactics";
        private const string MODS_FOLDER = "Mods";
        private const string ICON_PATH = "manifest.png";

        /// <summary>
        /// UI信息显示
        /// </summary>
        [ObservableProperty]
        public string info = string.Empty;
        /// <summary>
        /// Mods列表
        /// </summary>
        public AvaloniaList<string> ModsList { get; private set; }

        #region Listbox 数据绑定
        private string? _selectedItem;
        public string? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    OnSelectionChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// 当前展示的MOD信息
        /// </summary>
        [ObservableProperty]
        public L_ModDesc? modDesc;
        /// <summary>
        /// 配置，用于读取当前选定的MOD
        /// </summary>
        private L_Config _config;
        /// <summary>
        /// 当前启用的MOD
        /// </summary>
        [ObservableProperty]
        public string currMod;
        [ObservableProperty]
        public Bitmap? selectedModIcon;
        #endregion

        public ModsManagePageViewModel(ILang lang, ILoggerContainer loggerContainer, IMessenger messenger,
            IOSTools oSTools)
        {
            Language = lang.Language;

            _logger = loggerContainer.Builder.GetCurrentClassLogger();
            _messenger = messenger;
            _oses = oSTools.OSes;
            _config = _oses.LoadConfig();
            CurrMod = _config.CurrentlyEnabledMOD;

            ModsList = [];

            CheckFolders();
            CheckLocalMods();
        }

        private void CheckFolders()
        {
            _oses.CheckOrCreateDir(TACTICS_FOLDER);
            _oses.CheckOrCreateDir(MODS_FOLDER);
            _oses.LoadConfig();
        }

        [RelayCommand]
        public void CheckLocalMods()
        {
            var files = Directory.GetFiles(MODS_FOLDER, "*.zip");
            var len = files.Length;
            Info = string.Format(Language.MODS_MANAGE_VIEW_LOCAL_MODS_INFO, len);

            if (len > 0)
            {
                ModsList.Clear();
                ModsList.AddRange(files);
            }

            CurrMod = _config.CurrentlyEnabledMOD;
            SelectedModIcon = null;
        }

        [RelayCommand]
        public void NaviBack()
        {
            _messenger.Send(new MB_NavigationBack());
        }

        [RelayCommand]
        public void OpenLocalFolder()
        {
            _oses.OpenUrl(MODS_FOLDER);
        }

        private void OnSelectionChanged()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedItem)) return;

                SelectedModIcon?.Dispose();

                using var mod = new ModPackage(SelectedItem);
                using var ms = new MemoryStream(mod.ReadBinaryFile(ICON_PATH));

                ModDesc = mod.ModDesc;
                SelectedModIcon = new Bitmap(ms);
            }
            catch (Exception ex)
            {
                var errMsg = string.Format(Language.MODS_MANAGE_VIEW_SELECTED_MOD_ERROR, SelectedItem, ex.Message);

                _messenger.Send(new MB_ToastPureText() 
                { 
                    Message = errMsg,
                    Title = Language.TOAST_TITLE_ERROR,
                    Type = MB_Enum_ToastType.Error
                });

                _logger.Error(errMsg);
            }
        }

        /// <summary>
        /// 启用选定的MOD
        /// </summary>
        [RelayCommand]
        public void EnableSelectedMod()
        {
            if (string.IsNullOrEmpty(SelectedItem)) return;

            _config.CurrentlyEnabledMOD = SelectedItem;
            _oses.SaveConfig();

            CheckLocalMods();
        }

        /// <summary>
        /// 删除选定的MOD
        /// </summary>
        [RelayCommand]
        public void DeleteSelectedMod()
        {
            if (string.IsNullOrEmpty(SelectedItem)) return;

            try
            {
                File.Delete(SelectedItem);
                CheckLocalMods();
            }
            catch (Exception ex)
            {
                var errMsg = string.Format(Language.MODS_MANAGE_VIEW_MOD_ACTION_DELETE_SELECTED_ERROR, SelectedItem, ex.Message);

                _messenger.Send(new MB_ToastPureText()
                {
                    Message = errMsg,
                    Title = Language.TOAST_TITLE_ERROR,
                    Type = MB_Enum_ToastType.Error
                });

                _logger.Error(errMsg);
            }
        }
    }
}
