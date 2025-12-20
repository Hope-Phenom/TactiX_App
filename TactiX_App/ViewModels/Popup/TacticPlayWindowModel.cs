using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Popup;

public partial class TacticPlayWindowModel : ViewModelBase, IRecipient<MbHotkey>
{
#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public TacticPlayWindowModel()
    {
    } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

    public TacticPlayWindowModel(ILang lang, ILoggerContainer loggerContainer, IMessenger messenger,
        IosTools oSTools)
    {
        Language = lang.Language;
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
        timeStampTxt = "00:00";
        currStepTimeStampTxt = "00:00";
        timeStampGapTxt = string.Empty;

        // 逻辑初始化
        TacticFiles = [];
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

    #region 快捷键消息处理

    public void Receive(MbHotkey message)
    {
        switch (message.HotkeyEnum)
        {
            case LHotkeyBindingEnum.StartOrResume:
                _isPause = !_isPause;
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

    public ILanguage Language { get; }
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
    [ObservableProperty] public bool isPrepare;

    /// <summary>
    ///     UI是否是迷你模式
    /// </summary>
    private bool _isMini;

    /// <summary>
    ///     UI的高度
    /// </summary>
    [ObservableProperty] public int uIHeight;

    /// <summary>
    ///     设置Group的分割线高度
    /// </summary>
    [ObservableProperty] public GridLength horizontalLineHeight;

    /// <summary>
    ///     设置Group的高度
    /// </summary>
    [ObservableProperty] public GridLength groupHeight;

    /// <summary>
    ///     下拉按钮的图标
    /// </summary>
    [ObservableProperty] public MaterialIconKind materialIconKind;

    /// <summary>
    ///     TitleBarImage
    /// </summary>
    [ObservableProperty] public IImage titleBarImage;

    public LConfig Config { get; }

    #endregion

    #region 数据绑定-核心播放逻辑相关

    /// <summary>
    ///     战术文件列表
    /// </summary>
    public AvaloniaList<string> TacticFiles { get; }

    /// <summary>
    ///     战术选择文本框数据绑定-内部
    /// </summary>
    private string? _selectedTacticFile;

    /// <summary>
    ///     战术选择文本框数据绑定
    /// </summary>
    public string? SelectedTacticFile
    {
        get => _selectedTacticFile;
        set
        {
            if (_selectedTacticFile != value)
            {
                _selectedTacticFile = value;
                OnPropertyChanged();
                OnSelectionChanged();
            }
        }
    }

    /// <summary>
    ///     当前的战术文件
    /// </summary>
    [ObservableProperty] public LTactic? currTactic;

    /// <summary>
    ///     时间戳文本
    /// </summary>
    [ObservableProperty] public string timeStampTxt;

    /// <summary>
    ///     当前步骤的标准时间
    /// </summary>
    [ObservableProperty] public string currStepTimeStampTxt;

    /// <summary>
    ///     当前与标准时间的差值
    /// </summary>
    [ObservableProperty] public string timeStampGapTxt;

    #endregion

    #region Command和事件响应

    /// <summary>
    ///     关闭当前播放窗体
    /// </summary>
    [RelayCommand]
    public void CloseWindow()
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
    public void PullWindow()
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

        if (CurrTactic.TacticType == LTacticEnum.Timeline)
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (s, e) => PlayNext();
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
    public void Refresh()
    {
        ListTacticFiles();
    }

    /// <summary>
    ///     打开当前的战术目录
    /// </summary>
    [RelayCommand]
    public void OpenFolder()
    {
        _oses.OpenUrl(_filePrefix);
    }

    /// <summary>
    ///     战术文件选择项发生变化
    /// </summary>
    private void OnSelectionChanged()
    {
        if (string.IsNullOrEmpty(SelectedTacticFile)) return;

        var filePath = Path.Combine(_filePrefix, SelectedTacticFile);

        if (!File.Exists(filePath)) return;

        try
        {
            CurrTactic = JsonConvert.DeserializeObject<LTactic>(File.ReadAllText(filePath));

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
                Title = Language.ToastTitleError,
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
    private void Resume()
    {
        _isPause = false;
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

            for (var slotNo = 0; slotNo < 5; slotNo++)
            {
                // 换算为对应的指针
                var index = _currIndex + slotNo - 2;

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
                    var itemName = _modItems.Where(i => i.Abbr == action.ItemAbbr).First().Desc;
                    var itemTime = ConvertTimeToHhmmStr(action.Time);
                    var desc = $"{itemName}{Environment.NewLine}{itemTime}";
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
                var desc = _modItems.Where(i => i.Abbr == action.ItemAbbr).First().Desc;
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
            _waveOut.PlaybackStopped += (e, a) => { memoryStream.Dispose(); };
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
        for (var i = 0; i < tactic.Actions.Count; i++)
        {
            var action = tactic.Actions[i];
            var type = _modItems.Where(item => item.Abbr == action.ItemAbbr).First().Type;
            if (conf[type]) list.Add(action);
        }

        tactic.Actions = list;
    }

    #endregion
}