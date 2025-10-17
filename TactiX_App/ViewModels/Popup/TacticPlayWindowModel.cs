using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Material.Icons;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace TactiX_App.ViewModels.Popup
{
    public partial class TacticPlayWindowModel : ViewModelBase
    {
        #region DI容器注入
        public ILanguage Language { get; private set; }
        private readonly Logger _logger;
        private readonly IMessenger _messenger;
        private readonly L_Config _config;
        private readonly IOSes _oses;
        #endregion

        #region 变量/常量
        private const string WINDOW_NAME = "TacticPlayWindow";
        private const string TITLE_BAR_IMAGE = "titlebar.png";
        private const string TACTICS_FOLDER = "Tactics";
        private const string TACTICS_SEARCH_PATTERN = "*.tactix";
        private readonly TimeSpan NORMAL_TIME_INTERVAL = new TimeSpan(0, 0, 0, 1, 0);
        private readonly TimeSpan REAL_TIME_INTERVAL = new TimeSpan(0, 0, 0, 0, 968);
        private readonly Point PLAYING_SIZE = new(700, 180);
        private readonly Point MINI_SIZE = new(700, 230);
        private readonly Point NORMAL_SIZE = new(700, 700);

        /// <summary>
        /// 当前装载的Mod
        /// </summary>
        private readonly ModPackage _modPackage;
        /// <summary>
        /// 战术文件的路径前缀
        /// </summary>
        private readonly string _filePrefix;
        #endregion

        #region 数据绑定-UI大小等控制
        /// <summary>
        /// UI是否是准备模式
        /// </summary>
        public bool IsPrepare { get; private set; }
        /// <summary>
        /// UI是否是迷你模式
        /// </summary>
        private bool IsMini;
        /// <summary>
        /// UI的高度
        /// </summary>
        [ObservableProperty]
        public int uIHeight;
        /// <summary>
        /// 设置Group的分割线高度
        /// </summary>
        [ObservableProperty]
        public GridLength horizontalLineHeight;
        /// <summary>
        /// 设置Group的高度
        /// </summary>
        [ObservableProperty]
        public GridLength groupHeight;
        /// <summary>
        /// 下拉按钮的图标
        /// </summary>
        [ObservableProperty]
        public MaterialIconKind materialIconKind;
        /// <summary>
        /// TitleBarImage
        /// </summary>
        [ObservableProperty]
        public IImage titleBarImage;
        #endregion

        #region 数据绑定-核心播放逻辑相关
        /// <summary>
        /// 战术文件列表
        /// </summary>
        public AvaloniaList<string> TacticFiles { get; private set; }
        /// <summary>
        /// 战术选择文本框数据绑定-内部
        /// </summary>
        private string? _selectedTacticFile;
        /// <summary>
        /// 战术选择文本框数据绑定
        /// </summary>
        public string? SelectedTacticFile
        {
            get => _selectedTacticFile;
            set 
            {
                if (_selectedTacticFile != value)
                {
                    _selectedTacticFile = value;
                    OnPropertyChanged(nameof(SelectedTacticFile));
                    OnSelectionChanged();
                }
            }
        }
        /// <summary>
        /// 当前的战术文件
        /// </summary>
        [ObservableProperty]
        public L_Tactic? currTactic;
        #endregion

        public TacticPlayWindowModel(ILang lang, ILoggerContainer loggerContainer, IMessenger messenger, 
            IOSTools oSTools) 
        {
            Language = lang.Language;
            _logger = loggerContainer.Builder.GetCurrentClassLogger();
            _messenger = messenger;
            _oses = oSTools.OSes;
            _config = _oses.LoadConfig();

            // 加载指定MOD
            _modPackage = new ModPackage(_config.CurrentlyEnabledMOD);

            // UI初始化
            IsPrepare = true;
            UIHeight = NORMAL_SIZE.Y;
            HorizontalLineHeight = new GridLength(10);
            GroupHeight = GridLength.Star;
            MaterialIconKind = MaterialIconKind.ArrowExpandUp;

            // 逻辑初始化
            TacticFiles = new AvaloniaList<string>();
            _filePrefix = Path.Combine(TACTICS_FOLDER, _modPackage.ModDesc!.TacticsPath);
            ListTacticFiles();

            using var ms = new MemoryStream(_modPackage.ReadBinaryFile(TITLE_BAR_IMAGE));
            titleBarImage = new Bitmap(ms);
        }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public TacticPlayWindowModel() { } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

        #region Command和事件响应
        /// <summary>
        /// 关闭当前播放窗体
        /// </summary>
        [RelayCommand]
        public void CloseWindow()
        {
            _messenger.Send(new MB_WindowClose() 
            { 
                Name = WINDOW_NAME
            });

            _modPackage.Dispose();
        }

        /// <summary>
        /// 拉起当前窗体
        /// </summary>
        [RelayCommand]
        public void PullWindow()
        {
            UIHeight = IsMini
                ? NORMAL_SIZE.Y
                : MINI_SIZE.Y;

            HorizontalLineHeight = IsMini
                ? new GridLength(10)
                : new GridLength(0);

            GroupHeight = IsMini
                ? GridLength.Star
                : new GridLength(0);

            MaterialIconKind = IsMini
                ? MaterialIconKind.ArrowExpandUp
                : MaterialIconKind.ArrowExpandDown;

            IsMini = !IsMini;
        }

        /// <summary>
        /// 开始播放指定的战术文件
        /// </summary>
        [RelayCommand]
        public void PlayTactic()
        {
        }

        /// <summary>
        /// 刷新战术列表
        /// </summary>
        [RelayCommand]
        public void Refresh()
        {
            ListTacticFiles();
        }

        /// <summary>
        /// 战术文件选择项发生变化
        /// </summary>
        private void OnSelectionChanged()
        {
            if (string.IsNullOrEmpty(SelectedTacticFile)) return;

            var filePath = Path.Combine(_filePrefix, SelectedTacticFile);

            if (!File.Exists(filePath)) return;

            try
            {
                CurrTactic = JsonConvert.DeserializeObject<L_Tactic>(File.ReadAllText(filePath));
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 其他实现逻辑
        /// <summary>
        /// 列出当前已有的战术文件
        /// </summary>
        private void ListTacticFiles()
        {
            _oses.CheckOrCreateDir(_filePrefix);

            var files = Directory.GetFiles(_filePrefix, TACTICS_SEARCH_PATTERN);
            if (files.Length > 0)
            {
                Dispatcher.UIThread.Invoke(() => 
                {
                    TacticFiles.Clear();
                    foreach (var file in files) 
                    {
                        TacticFiles.Add(file
                            .Replace(_filePrefix, string.Empty)
                            .Replace("\\", string.Empty)
                            .Replace("/", string.Empty));
                    }
                });
            }
        }
        #endregion
    }
}
