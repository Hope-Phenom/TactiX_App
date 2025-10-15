using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels
{
    public partial class ModsManageViewModel : ViewModelBase
    {
        #region DI注入
        public ILanguage Language { get; private set; }
        private readonly Logger _logger;
        private readonly IMessenger _messenger;
        private readonly IOSes _oses;
        #endregion

        #region 常量/变量
        private const string TacitcsFolder = "Tactics";
        private const string ModsFolder = "Mods";

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
        #endregion

        public ModsManageViewModel(ILang lang, ILoggerContainer loggerContainer, IMessenger messenger,
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
            _oses.CheckOrCreateDir(TacitcsFolder);
            _oses.CheckOrCreateDir(ModsFolder);
            _oses.LoadConfig();
        }

        [RelayCommand]
        public void CheckLocalMods()
        {
            var files = Directory.GetFiles(ModsFolder, "*.zip");
            var len = files.Length;
            Info = string.Format(Language.MODS_MANAGE_VIEW_LOCAL_MODS_INFO, len);

            if (len > 0)
            {
                ModsList.Clear();
                ModsList.AddRange(files);
            }
        }

        [RelayCommand]
        public void NaviBack()
        {
            _messenger.Send(new MB_NavigationBack());
        }

        [RelayCommand]
        public void OpenLocalFolder()
        {
            _oses.OpenUrl(ModsFolder);
        }

        private void OnSelectionChanged()
        {
            if (string.IsNullOrEmpty(_selectedItem)) return;

            using (var mod = new ModPackage(_selectedItem))
            {
                ModDesc = mod.ModDesc;
            }
        }
    }
}
