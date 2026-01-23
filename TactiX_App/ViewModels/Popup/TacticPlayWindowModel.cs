using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Material.Icons;
using NAudio.Wave;
using Newtonsoft.Json;
using NLog;
using TactiX_Logger;
using TactiX_Localization;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Popup;

public partial class TacticPlayWindowModel : ViewModelBase, IRecipient<MbHotkey>
{
#if DEBUG
#pragma warning disable CS8618
    public TacticPlayWindowModel()
    {
    }
#pragma warning restore CS8618
#endif

    public TacticPlayWindowModel(ILocalizationService localizationService, ILoggerContainer loggerContainer,
        IMessenger messenger, IosTools oSTools)
    {
        _localizationService = localizationService;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _messenger = messenger;
        _oses = oSTools.OSes;
        Config = _oses.LoadConfig();

        // 加载指定MOD
        _modPackage = new ModPackage(Config.CurrentlyEnabledMod);
        _modResourceCache = new ModResourceCache<Bitmap>(_modPackage, ms => new Bitmap(ms));

        // UI初始化
        SwtichToNormalMode();
        IsPrepare = true;
        TimeStampTxt = "00:00";
        CurrStepTimeStampTxt = "00:00";
        TimeStampGapTxt = string.Empty;
        FileSelectedInfo = _localizationService.GetString("TacticPlayingInfoUnselect");
        HotkeyTips = _localizationService.GetString("TacticPlayingInfoFileTips");
        HotkeyTipsDesc = string.Empty;

        // 逻辑初始化
        TacticFiles = [];
        _tactics = [];
        _filePrefix = Path.Combine(TACTICS_FOLDER, _modPackage.ModDesc!.TacticsPath);
        ListTacticFiles();
        
        // 预读文件
        if (Config.EnablePreload) Task.Run(LoadTacticFiles);

        _modItems = [.. _modPackage.ModDesc.Actions, .. _modPackage.ModDesc.Units];
        _isPause = false;
        _timeStamp = 0;
        _runningTimeStamp = 0;
        _timeStampGap = 0;

        using var ms = new MemoryStream(_modPackage.ReadBinaryFile(TITLE_BAR_IMAGE));
        _titleBarImage = new Bitmap(ms);

        _messenger.RegisterAll(this);
    }

    #region 快捷键消息处理

    public void Receive(MbHotkey message)
    {
        switch (message.HotkeyEnum)
        {
            case LHotkeyBindingEnum.StartOrResume:
                PauseOrResume();
                break;
            case LHotkeyBindingEnum.Stop:
                StopPlayback();
                SwtichToNormalMode();
                break;
            case LHotkeyBindingEnum.Previous:
                Pause();
                MovePrevious();
                break;
            case LHotkeyBindingEnum.Next:
                Pause();
                MoveNext();
                break;
        }
    }

    #endregion

    #region DI容器注入

    private readonly ILocalizationService _localizationService;
    private readonly Logger _logger;
    private readonly IMessenger _messenger;
    private readonly IoSes _oses;

    #endregion

    #region 变量/常量

    private const string WINDOW_NAME = "TacticPlayWindow";
    private const string TITLE_BAR_IMAGE = "titlebar.png";
    private const string TACTICS_FOLDER = "Tactics";
    private const string TACTICS_SEARCH_PATTERN = "*.tactix";
    private const string ICON_FOLDER = "icons";
    private const string SOUND_FOLDER = "sounds";
    private readonly TimeSpan _normalTimeInterval = new(0, 0, 0, 1, 0);
    private readonly TimeSpan _realTimeInterval = new(0, 0, 0, 0, 968);
    private readonly Point _playingSize = new(700, 180);
    private readonly Point _miniSize = new(700, 230);
    private readonly Point _normalSize = new(700, 700);

    /// <summary>
    ///     当前装载的Mod
    /// </summary>
    private readonly ModPackage _modPackage;

    /// <summary>
    ///     当前装载的Mod的缓存
    /// </summary>
    private readonly ModResourceCache<Bitmap> _modResourceCache;

    /// <summary>
    ///     战术文件的路径前缀
    /// </summary>
    private readonly string _filePrefix;

    /// <summary>
    ///     定时器
    /// </summary>
    private DispatcherTimer? _dispatcherTimer;

    /// <summary>
    ///     播放的序号指针
    /// </summary>
    private int _currIndex;

    /// <summary>
    ///     Mod中的对象列表（Action和Unit合并）
    /// </summary>
    private readonly List<LModItem> _modItems;

    /// <summary>
    ///     是否是暂停模式
    /// </summary>
    private bool _isPause;

    /// <summary>
    ///     运行的时间戳（受暂停影响）
    /// </summary>
    private uint _timeStamp;

    /// <summary>
    ///     战术播放时间戳（不受暂停影响）
    /// </summary>
    private uint _runningTimeStamp;

    /// <summary>
    ///     运行事件和战术播放时间的差值
    /// </summary>
    private uint _timeStampGap;

    private WaveOutEvent? _waveOut;
    private WaveFileReader? _waveReader;

    #endregion

    #region 数据绑定-UI大小等控制

    /// <summary>
    ///     UI是否是准备模式
    /// </summary>
    [ObservableProperty] private bool _isPrepare;

    /// <summary>
    ///     UI是否是迷你模式
    /// </summary>
    private bool _isMini;

    /// <summary>
    ///     UI的高度
    /// </summary>
    [ObservableProperty] private int _uIHeight;

    /// <summary>
    ///     设置Group的分割线高度
    /// </summary>
    [ObservableProperty] private GridLength _horizontalLineHeight;

    /// <summary>
    ///     设置Group的高度
    /// </summary>
    [ObservableProperty] private GridLength _groupHeight;

    /// <summary>
    ///     下拉按钮的图标
    /// </summary>
    [ObservableProperty] private MaterialIconKind _materialIconKind;

    /// <summary>
    ///     TitleBarImage
    /// </summary>
    [ObservableProperty] private IImage _titleBarImage;

    /// <summary>
    ///     文件选择情况提示
    /// </summary>
    [ObservableProperty] private string _fileSelectedInfo;

    /// <summary>
    ///     热键提示（左半部分）
    /// </summary>
    [ObservableProperty] private string _hotkeyTips;

    /// <summary>
    ///     热键提示（右半部分）
    /// </summary>
    [ObservableProperty] private string _hotkeyTipsDesc;

    public LConfig Config { get; }

    /// <summary>
    ///     透明度代理属性，用于绑定到UI
    /// </summary>
    public double Opacity
    {
        get => Config.Opacity;
        set
        {
            if (Config.Opacity != value)
            {
                Config.Opacity = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region 数据绑定-核心播放逻辑相关

    /// <summary>
    ///     战术文件列表
    /// </summary>
    public AvaloniaList<string> TacticFiles { get; }

    /// <summary>
    ///     战术选择文本框数据绑定
    /// </summary>
    public string? SelectedTacticFile
    {
        get;
        set
        {
            if (field == value) return;

            field = value;
            OnPropertyChanged();
            OnSelectionChanged();
        }
    }

    /// <summary>
    ///     当前的战术文件
    /// </summary>
    [ObservableProperty] private LTactic? _currTactic;

    /// <summary>
    ///     时间戳文本
    /// </summary>
    [ObservableProperty] private string _timeStampTxt;

    /// <summary>
    ///     当前步骤的标准时间
    /// </summary>
    [ObservableProperty] private string _currStepTimeStampTxt;

    /// <summary>
    ///     当前与标准时间的差值
    /// </summary>
    [ObservableProperty] private string _timeStampGapTxt;

    /// <summary>
    /// 加载所有的战术文件
    /// </summary>
    private readonly List<LTactic?> _tactics;

    #endregion

    #region Command和事件响应

    /// <summary>
    ///     关闭当前播放窗体
    /// </summary>
    [RelayCommand]
    private void CloseWindow()
    {
        _messenger.Send(new MbWindowClose
        {
            Name = WINDOW_NAME
        });

        _modPackage.Dispose();
    }

    /// <summary>
    ///     拉起当前窗体
    /// </summary>
    [RelayCommand]
    private void PullWindow()
    {
        _isMini = !_isMini;

        UIHeight = _isMini
            ? _miniSize.Y
            : _normalSize.Y;

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
    ///     开始播放指定的战术文件
    /// </summary>
    [RelayCommand]
    private void PlayTactic()
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

        if (CurrTactic.TacticType == LTacticEnum.Timeline)
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (_, _) => PlayNext();
            _dispatcherTimer.Interval = Config.EnableTlCorr
                ? _realTimeInterval
                : _normalTimeInterval;
            BoardCastCurrStep();
            PlayNext();
            _dispatcherTimer.Start();
        }
        else
        {
            BoardCastCurrStep();
            PlayNext();
        }
    }

    /// <summary>
    ///     刷新战术列表
    /// </summary>
    [RelayCommand]
    private void Refresh()
    {
        ListTacticFiles();
        if (Config.EnablePreload) Task.Run(LoadTacticFiles);
    }

    /// <summary>
    ///     打开当前的战术目录
    /// </summary>
    [RelayCommand]
    private void OpenFolder()
    {
        _oses.OpenUrl(_filePrefix);
    }

    /// <summary>
    ///     战术文件选择项发生变化
    /// </summary>
    private void OnSelectionChanged()
    {
        try
        {
            if (string.IsNullOrEmpty(SelectedTacticFile)) return;

            FileSelectedInfo = Path.GetFileNameWithoutExtension(SelectedTacticFile);

            if (Config.EnablePreload)
            {
                CurrTactic = _tactics[TacticFiles.IndexOf(SelectedTacticFile)];
            }
            else
            {
                var filePath = Path.Combine(_filePrefix, SelectedTacticFile);
                if (!File.Exists(filePath)) return;

                CurrTactic = JsonConvert.DeserializeObject<LTactic>(File.ReadAllText(filePath));
            }

            if (CurrTactic == null) return;
            
            SortActionsByModItemType(CurrTactic);
        }
        catch (Exception ex)
        {
            _messenger.Send(new MbWindowStatus
            {
                WindowStatus = MbWindowStatus.MbEnumWindowStatus.Normal
            });

            _messenger.Send(new MbToastPureText
            {
                Message = ex.Message,
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });
        }
    }

    /// <summary>
    ///     切换到播放模式
    /// </summary>
    private void SwtichToPlayingMode()
    {
        UIHeight = _playingSize.Y;

        HotkeyTips = string.Empty;
        HotkeyTipsDesc = GetHotkeyStr();

        _messenger.Send(new MbWindowPointerTrans
        {
            Enable = true
        });
    }

    /// <summary>
    ///     切换到正常模式
    /// </summary>
    private void SwtichToNormalMode()
    {
        UIHeight = _normalSize.Y;
        HorizontalLineHeight = new GridLength(10);
        GroupHeight = GridLength.Star;
        MaterialIconKind = MaterialIconKind.ArrowExpandUp;
        _isMini = false;

        HotkeyTips = _localizationService.GetString("TacticPlayingInfoFileTips");
        HotkeyTipsDesc = string.Empty;

        _messenger.Send(new MbWindowPointerTrans
        {
            Enable = false
        });
    }

    #endregion

    #region 战术播放逻辑

    /// <summary>
    ///     播放战术到下一步
    /// </summary>
    private void PlayNext()
    {
        try
        {
            if (CurrTactic == null) return;

            var actions = CurrTactic.Actions;
            var timeLineMode = CurrTactic.TacticType == LTacticEnum.Timeline;

            if (_currIndex < actions.Count - 1)
            {
                if (timeLineMode)
                {
                    if (_timeStamp == actions[_currIndex + 1].Time && !_isPause)
                    {
                        _currIndex++;
                        UpdateTacticTimeStamp();
                        BoardCastCurrStep();
                        PlayWav();
                    }

                    UpdateTimeStamp();
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
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    /// <summary>
    ///     停止播放
    /// </summary>
    private void StopPlayback()
    {
        _dispatcherTimer?.Stop();
    }

    /// <summary>
    ///     手动播放上一步
    /// </summary>
    private void MovePrevious()
    {
        try
        {
            if (CurrTactic == null) return;

            if (_currIndex > 0) _currIndex--;

            UpdateTacticTimeStamp();
            BoardCastCurrStep();
            PlayWav();
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    /// <summary>
    ///     手动播放下一步
    /// </summary>
    private void MoveNext()
    {
        try
        {
            if (CurrTactic == null) return;

            var actions = CurrTactic!.Actions;

            if (_currIndex < actions.Count - 1) _currIndex++;

            UpdateTacticTimeStamp();
            BoardCastCurrStep();
            PlayWav();
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    /// <summary>
    ///     暂停
    /// </summary>
    private void Pause()
    {
        _isPause = true;
    }

    /// <summary>
    ///     恢复播放
    /// </summary>
    private void PauseOrResume()
    {
        _isPause = !_isPause;
    }

    /// <summary>
    ///     广播当前的步骤显示内容到Item
    /// </summary>
    private void BoardCastCurrStep()
    {
        try
        {
            if (CurrTactic == null) return;
            if (_modPackage.ModDesc == null) return;

            for (var slotNo = 0; slotNo < 4; slotNo++)
            {
                // 换算为对应的指针
                var index = _currIndex + slotNo - 1;

                // 超出了范围，让Item显示为空
                if (index < 0 || index >= CurrTactic.Actions.Count)
                {
                    _messenger.Send(new MbDisplayStep
                    {
                        SlotNo = slotNo
                    });
                }
                // 范围内，广播显示内容
                else
                {
                    var action = CurrTactic.Actions[index];
                    var image = _modResourceCache.GetImage(Path.Combine(ICON_FOLDER, $"{action.ItemAbbr}.png"));
                    var itemName = _modItems.First(i => i.Abbr == action.ItemAbbr).Desc;
                    var itemTime = ConvertTimeToHhmmStr(action.Time);
                    var number = action.Number > 1
                        ? $" x{action.Number}"
                        : string.Empty;
                    var desc = $"{itemName}{number}{Environment.NewLine}{itemTime}";
                    var supply = action.Supply;

                    _messenger.Send(new MbDisplayStep
                    {
                        SlotNo = slotNo,
                        Desc = desc,
                        Image = image,
                        Supply = supply
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    /// <summary>
    ///     重置所有显示槽位
    /// </summary>
    private void ClearSlots()
    {
        for (var i = 0; i < 5; i++)
            _messenger.Send(new MbDisplayStep
            {
                SlotNo = i
            });
    }

    /// <summary>
    ///     将槽位设置为播放到第一个节点的状态
    /// </summary>
    private void ResetSlots()
    {
        try
        {
            ClearSlots();

            if (CurrTactic == null || CurrTactic.Actions.Count < 3) return;

            for (var slotNo = 3; slotNo < 5; slotNo++)
            {
                var action = CurrTactic.Actions[slotNo - 3];
                var image = _modResourceCache.GetImage(Path.Combine(ICON_FOLDER, $"{action.ItemAbbr}.png"));
                var desc = _modItems.First(i => i.Abbr == action.ItemAbbr).Desc;
                _messenger.Send(new MbDisplayStep
                {
                    SlotNo = slotNo,
                    Desc = desc,
                    Image = image
                });
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    /// <summary>
    ///     更新时间戳
    /// </summary>
    private void UpdateTimeStamp()
    {
        try
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

            TimeStampTxt = ConvertTimeToHhmmStr(_runningTimeStamp);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    /// <summary>
    ///     更新战术时间戳
    /// </summary>
    private void UpdateTacticTimeStamp()
    {
        try
        {
            if (CurrTactic == null || CurrTactic.TacticType == LTacticEnum.Step) return;

            var timestamp = CurrTactic.Actions[_currIndex].Time;

            CurrStepTimeStampTxt = ConvertTimeToHhmmStr(timestamp);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    /// <summary>
    ///     拼接时间字符串
    /// </summary>
    /// <returns></returns>
    private static string CombineTimeStr(uint min, uint sec)
    {
        return min.ToString().PadLeft(2, '0') + ":" + sec.ToString().PadLeft(2, '0');
    }

    /// <summary>
    ///     将秒数转成HHMM格式的字符串
    /// </summary>
    private static string ConvertTimeToHhmmStr(uint time)
    {
        var sec = time % 60;
        var min = (time - sec) / 60;
        return CombineTimeStr(min, sec);
    }

    #endregion

    #region 其他实现逻辑

    /// <summary>
    ///     列出当前已有的战术文件
    /// </summary>
    private void ListTacticFiles()
    {
        _oses.CheckOrCreateDir(_filePrefix);

        var files = Directory.GetFiles(_filePrefix, TACTICS_SEARCH_PATTERN);
        if (files.Length > 0)
            Dispatcher.UIThread.Invoke(() =>
            {
                TacticFiles.Clear();
                foreach (var file in files)
                    TacticFiles.Add(file
                        .Replace(_filePrefix, string.Empty)
                        .Replace("\\", string.Empty)
                        .Replace("/", string.Empty));
            });
    }

    /// <summary>
    ///     一次性读取当前所有的战术文件，提升切换选择时的性能
    /// </summary>
    private void LoadTacticFiles()
    {
        _tactics.Clear();
        
        var files = Directory.GetFiles(_filePrefix, TACTICS_SEARCH_PATTERN);
        foreach (var file in files)
        {
            _tactics.Add(JsonConvert.DeserializeObject<LTactic>(File.ReadAllText(file)));
        }
    }

    private void PlayWav()
    {
        try
        {
            if (CurrTactic == null) return;

            var action = CurrTactic.Actions[_currIndex];
            var wav = _modResourceCache.GetAudio(Path.Combine(SOUND_FOLDER, $"{action.ItemAbbr}.wav"));
            PlayWavFromMemoryStream(wav);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }
    }

    private void PlayWavFromMemoryStream(byte[] wavData)
    {
        try
        {
            StopWav();

            var memoryStream = new MemoryStream(wavData);
            _waveReader = new WaveFileReader(memoryStream);
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_waveReader);
            _waveOut.PlaybackStopped += (_, _) => { memoryStream.Dispose(); };
            _waveOut.Play();
        }
        catch (Exception ex)
        {
            StopWav();
            _logger.Error(ex.ToString());
        }
    }

    private void StopWav()
    {
        _waveOut?.Stop();
        _waveOut?.Dispose();
        _waveReader?.Dispose();
        _waveOut = null;
        _waveReader = null;
    }

    /// <summary>
    ///     根据配置筛选战术动作
    /// </summary>
    private void SortActionsByModItemType(LTactic tactic)
    {
        var conf = Config.ModItemTypeEnable;
        var list = new List<LTacticAction>();
        foreach (var action in tactic.Actions)
        {
            var type = _modItems
                .First(item => item.Abbr == action.ItemAbbr)
                .Type;
            if (conf[type]) list.Add(action);
        }

        tactic.Actions = list;
    }

    /// <summary>
    ///     从配置文件中获得快捷键的文本
    /// </summary>
    /// <returns>快捷键的文本</returns>
    private string GetHotkeyStr()
    {
        if (Config.Hotkeys.Length != 4) return string.Empty;

        var baseStr = _localizationService.GetString("TacticPlayingInfoHotkeyDesc");
        return string.Format(baseStr,
            $"{Config.Hotkeys[0].Modifiers}+{Config.Hotkeys[0].Key}",
            $"{Config.Hotkeys[1].Modifiers}+{Config.Hotkeys[1].Key}",
            $"{Config.Hotkeys[2].Modifiers}+{Config.Hotkeys[2].Key}",
            $"{Config.Hotkeys[3].Modifiers}+{Config.Hotkeys[3].Key}");
    }

    #endregion
}