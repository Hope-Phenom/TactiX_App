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
using static System.Net.Mime.MediaTypeNames;

namespace TactiX_App.ViewModels.Popup
{
    public partial class TacticPlayWindowModel : ViewModelBase, IRecipient<MB_Hotkey>
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
        private const string ICON_FOLDER = "icons";
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
        /// 当前装载的Mod的缓存
        /// </summary>
        private readonly ModResourceCache<Bitmap> _modResourceCache;
        /// <summary>
        /// 战术文件的路径前缀
        /// </summary>
        private readonly string _filePrefix;
        /// <summary>
        /// 定时器
        /// </summary>
        private DispatcherTimer? _dispatcherTimer;
        /// <summary>
        /// 播放的序号指针
        /// </summary>
        private int _currIndex;
        /// <summary>
        /// Mod中的对象列表（Action和Unit合并）
        /// </summary>
        private List<L_ModItem> _modItems;
        /// <summary>
        /// 是否是暂停模式
        /// </summary>
        private bool _isPause;
        /// <summary>
        /// 运行的时间戳（受暂停影响）
        /// </summary>
        private uint _timeStamp;
        /// <summary>
        /// 战术播放时间戳（不受暂停影响）
        /// </summary>
        private uint _runningTimeStamp;
        /// <summary>
        /// 运行事件和战术播放时间的差值
        /// </summary>
        private uint _timeStampGap;
        #endregion

        #region 数据绑定-UI大小等控制
        /// <summary>
        /// UI是否是准备模式
        /// </summary>
        [ObservableProperty]
        public bool isPrepare;
        /// <summary>
        /// UI是否是迷你模式
        /// </summary>
        private bool _isMini;
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
        public L_Config Config => _config;
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
        /// <summary>
        /// 时间戳文本
        /// </summary>
        [ObservableProperty]
        public string timeStampTxt;
        /// <summary>
        /// 当前步骤的标准时间
        /// </summary>
        [ObservableProperty]
        public string currStepTimeStampTxt;
        /// <summary>
        /// 当前与标准时间的差值
        /// </summary>
        [ObservableProperty]
        public string timeStampGapTxt;
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
            _modResourceCache = new ModResourceCache<Bitmap>(_modPackage, (ms) => new Bitmap(ms));

            // UI初始化
            SwtichToNormalMode();
            IsPrepare = true;
            timeStampTxt = "00:00";
            currStepTimeStampTxt = "00:00";
            timeStampGapTxt = string.Empty;

            // 逻辑初始化
            TacticFiles = new AvaloniaList<string>();
            _filePrefix = Path.Combine(TACTICS_FOLDER, _modPackage.ModDesc!.TacticsPath);
            ListTacticFiles();

            _modItems = [.. _modPackage.ModDesc.Actions, .. _modPackage.ModDesc.Units];
            _isPause = false;
            _timeStamp = 0;
            _runningTimeStamp = 0;
            _timeStampGap = 0;

            using var ms = new MemoryStream(_modPackage.ReadBinaryFile(TITLE_BAR_IMAGE));
            titleBarImage = new Bitmap(ms);

            _messenger.RegisterAll(this);
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
            _isMini = !_isMini;

            UIHeight = _isMini
                ? MINI_SIZE.Y
                : NORMAL_SIZE.Y;

            HorizontalLineHeight = _isMini
                ? new GridLength(0)
                : new GridLength(10);

            GroupHeight = _isMini
                ? new GridLength(0)
                : GridLength.Star;

            MaterialIconKind = _isMini
                ? MaterialIconKind.ArrowExpandDown
                : MaterialIconKind.ArrowExpandUp;
        }

        /// <summary>
        /// 开始播放指定的战术文件
        /// </summary>
        [RelayCommand]
        public void PlayTactic()
        {
            if (CurrTactic == null) return;

            IsPrepare = false;
            ResetSlots();

            _isPause = false;
            _timeStamp = 0;
            _runningTimeStamp = 0;
            _timeStampGap = 0;
            _currIndex = -1;

            SwtichToPlayingMode();

            if (CurrTactic.TacticType == L_TacticEnum.TIMELINE)
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Tick += (s, e) => PlayNext();
                _dispatcherTimer.Interval = Config.EnableTLCorr
                    ? REAL_TIME_INTERVAL
                    : NORMAL_TIME_INTERVAL;
                PlayNext();
                _dispatcherTimer.Start();
            }
            else
            {
                PlayNext();
            }
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
            catch (Exception ex)
            {
                _messenger.Send(new MB_WindowStatus()
                {
                    WindowStatus = MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Normal
                });

                _messenger.Send(new MB_ToastPureText()
                {
                    Message = ex.Message,
                    Title = Language.TOAST_TITLE_ERROR,
                    Type = MB_Enum_ToastType.Error
                });
            }
        }
        /// <summary>
        /// 切换到播放模式
        /// </summary>
        private void SwtichToPlayingMode()
        {
            UIHeight = PLAYING_SIZE.Y;
            _messenger.Send(new MB_WindowPointerTrans()
            {
                Enable = true
            });
        }
        /// <summary>
        /// 切换到正常模式
        /// </summary>
        private void SwtichToNormalMode()
        {
            UIHeight = NORMAL_SIZE.Y;
            HorizontalLineHeight = new GridLength(10);
            GroupHeight = GridLength.Star;
            MaterialIconKind = MaterialIconKind.ArrowExpandUp;
            _isMini = false;

            _messenger.Send(new MB_WindowPointerTrans()
            {
                Enable = false
            });
        }
        #endregion

        #region 战术播放逻辑
        /// <summary>
        /// 播放战术到下一步
        /// </summary>
        private void PlayNext()
        {
            if (CurrTactic == null) return;

            var actions = CurrTactic.Actions;
            var timeLineMode = CurrTactic.TacticType == L_TacticEnum.TIMELINE;

            if (_currIndex < actions.Count - 1)
            {
                if (timeLineMode)
                {
                    UpdateTimeStamp();

                    if (_timeStamp == actions[_currIndex + 1].Time && !_isPause)
                    {
                        _currIndex++;
                        UpdateTacticTimeStamp();
                        BoardCastCurrStep();
                    }
                }
                else
                {
                    _currIndex++;
                    BoardCastCurrStep();
                }
            }
            else
            {
                StopPlayback();
            }
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        private void StopPlayback()
        {
            _dispatcherTimer?.Stop();
        }
        /// <summary>
        /// 手动播放上一步
        /// </summary>
        private void MovePrevious()
        {
            if (CurrTactic == null) return;

            if (_currIndex > 0) _currIndex--;

            UpdateTacticTimeStamp();
            BoardCastCurrStep();
        }
        /// <summary>
        /// 手动播放下一步
        /// </summary>
        private void MoveNext()
        {
            if (CurrTactic == null) return;

            var actions = CurrTactic!.Actions;

            if (_currIndex < actions.Count - 1) _currIndex++;

            UpdateTacticTimeStamp();
            BoardCastCurrStep();
        }
        /// <summary>
        /// 暂停
        /// </summary>
        private void Pause()
        {
            _isPause = true;
        }
        /// <summary>
        /// 恢复播放
        /// </summary>
        private void Resume()
        {
            _isPause = false;
        }
        /// <summary>
        /// 广播当前的步骤显示内容到Item
        /// </summary>
        private void BoardCastCurrStep()
        {
            if (CurrTactic == null) return;
            if (_modPackage.ModDesc == null) return;

            for (int slotNo = 0; slotNo < 5; slotNo++)
            {
                // 换算为对应的指针
                var index = _currIndex + slotNo - 2;

                // 超出了范围，让Item显示为空
                if (index < 0 || index >= CurrTactic.Actions.Count)
                {
                    _messenger.Send(new MB_DisplayStep()
                    {
                        SlotNo = slotNo
                    });
                }
                // 范围内，广播显示内容
                else
                {
                    var action = CurrTactic.Actions[index];
                    var image = _modResourceCache.GetImage(Path.Combine(ICON_FOLDER, $"{action.ItemAbbr}.png"));
                    var desc = _modItems.Where(i => i.Abbr == action.ItemAbbr).First().Desc;

                    _messenger.Send(new MB_DisplayStep()
                    {
                        SlotNo = slotNo,
                        Desc = desc,
                        Image = image
                    });
                }
            }
        }
        /// <summary>
        /// 重置所有显示槽位
        /// </summary>
        private void ClearSlots()
        {
            for (int i = 0; i < 5; i++)
            {
                _messenger.Send(new MB_DisplayStep()
                {
                    SlotNo = i
                });
            }
        }
        /// <summary>
        /// 将槽位设置为播放到第一个节点的状态
        /// </summary>
        private void ResetSlots()
        {
            ClearSlots();

            if (CurrTactic == null || CurrTactic.Actions.Count < 3) return;

            for (int slotNo = 3; slotNo < 5; slotNo++)
            {
                var action = CurrTactic.Actions[slotNo - 3];
                var image = _modResourceCache.GetImage(Path.Combine(ICON_FOLDER, $"{action.ItemAbbr}.png"));
                var desc = _modItems.Where(i => i.Abbr == action.ItemAbbr).First().Desc;
                _messenger.Send(new MB_DisplayStep()
                {
                    SlotNo = slotNo,
                    Desc = desc,
                    Image = image
                });
            }
        }
        /// <summary>
        /// 更新时间戳
        /// </summary>
        private void UpdateTimeStamp()
        {
            if (!_isPause) _timeStamp++;

            _runningTimeStamp++;
            _timeStampGap = _runningTimeStamp - _timeStamp;

            if (_timeStampGap != 0)
            {
                var symbol = _timeStampGap > 0 ? "+" : "-";
                TimeStampGapTxt = $"{symbol}{_timeStampGap} s";
            }
            else
            {
                TimeStampGapTxt = string.Empty;
            }

            var sec = _runningTimeStamp % 60;
            var min = (_runningTimeStamp - sec) / 60;
            TimeStampTxt = CombineTimeStr(min, sec);
        }
        /// <summary>
        /// 更新战术时间戳
        /// </summary>
        private void UpdateTacticTimeStamp()
        {
            if (CurrTactic == null || CurrTactic.TacticType == L_TacticEnum.STEP) return;

            var timestamp = CurrTactic.Actions[_currIndex].Time;

            var sec = timestamp % 60;
            var min = (timestamp - sec) / 60;
            CurrStepTimeStampTxt = CombineTimeStr(min, sec);
        }
        /// <summary>
        /// 拼接时间字符串
        /// </summary>
        /// <returns></returns>
        private string CombineTimeStr(uint min, uint sec)
        {
            return min.ToString().PadLeft(2, '0') + ":" + sec.ToString().PadLeft(2, '0');
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

        #region 快捷键消息处理
        public void Receive(MB_Hotkey message)
        {
            switch (message.HotkeyEnum)
            {
                case L_HotkeyBindingEnum.StartOrResume:
                    _isPause = !_isPause;
                    break;
                case L_HotkeyBindingEnum.Stop:
                    StopPlayback();
                    SwtichToNormalMode();
                    break;
                case L_HotkeyBindingEnum.Previous:
                    Pause();
                    MovePrevious();
                    break;
                case L_HotkeyBindingEnum.Next:
                    Pause();
                    MoveNext();
                    break;
            }
        }
        #endregion
    }
}
