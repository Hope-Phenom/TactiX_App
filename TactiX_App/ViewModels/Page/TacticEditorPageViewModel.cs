using System;
using System.IO;
using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using NLog;
using TactiX_Localization;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page;

public partial class TacticEditorPageViewModel : ViewModelBase, IRecipient<MbFileDialog>
{
    public TacticEditorPageViewModel(IMessenger messenger, ILocalizationService localizationService, ITactiXSourceEncoder encoder,
        IosTools oSTools, ILoggerContainer loggerContainer)
    {
        _localizationService = localizationService;
        _messenger = messenger;
        _encoder = encoder;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        _logger = loggerContainer.Builder.GetCurrentClassLogger();

        _messenger.RegisterAll(this);

        TextDocument = new TextDocument();

        CreateNewFile();
    }

    #region 消息处理

    public void Receive(MbFileDialog message)
    {
        if (string.IsNullOrEmpty(message.FilePath)) return;
        if (message.WindowName != MAIN_WINDOW) return;
        if (message.Trigger != EXPORT_TRIGGER
            && message.Trigger != TACEDIT_TRIGGER) return;

        if (message.Trigger == EXPORT_TRIGGER)
            _exportPath = message.FilePath;
        else if (message.Trigger == TACEDIT_TRIGGER) _filePath = message.FilePath;

        if (message.IsOpenMode)
            OpenFileHandle();
        else
            SaveFileHandle(message.Trigger);
    }

    #endregion

    #region DI容器注入

    private readonly ILocalizationService _localizationService;
    private readonly IMessenger _messenger;
    private readonly ITactiXSourceEncoder _encoder;
    private readonly IoSes _oses;
    private readonly LConfig _config;
    private readonly ILogger _logger;

    #endregion

    #region 常量

    private const string TACTICS_FOLDER = "Tactics";
    private const string TACTIC_TEMPLATE_NAME = "TacticTemplate.tactixSource";
    private const string TITLE_HEADER = "TactiX - ";
    private const string NEW_FILE = "New File";
    private const string MAIN_WINDOW = "MainWindow";
    private const string EXPORT_TRIGGER = "Export";
    private const string TACEDIT_TRIGGER = "TacEdit";

    #endregion

    #region 逻辑变量

    private string _filePath = string.Empty;
    private string _exportPath = string.Empty;

    #endregion

    #region 数据绑定

    /// <summary>
    ///     编辑器中的文本
    /// </summary>
    public TextDocument TextDocument { get; }

    #endregion


    #region UI响应的Commands

    /// <summary>
    ///     创建新文件
    /// </summary>
    [RelayCommand]
    public void CreateNewFile()
    {
        _filePath = string.Empty;

        TextDocument.Text = File.Exists(TACTIC_TEMPLATE_NAME)
            ? File.ReadAllText(TACTIC_TEMPLATE_NAME)
            : string.Empty;

        _messenger.Send(new MbWindowTitle
        {
            Title = $"{TITLE_HEADER}{NEW_FILE}",
            WindowName = MAIN_WINDOW
        });
    }

    /// <summary>
    ///     打开文件
    /// </summary>
    [RelayCommand]
    public void OpenFile()
    {
        _messenger.Send(new MbFileDialog
        {
            WindowName = MAIN_WINDOW,
            IsOpenMode = true,
            FileFilterName = "TactiX Source",
            FileFilter = "*.tactixSource",
            Trigger = TACEDIT_TRIGGER
        });
    }

    /// <summary>
    ///     保存文件
    /// </summary>
    [RelayCommand]
    public void SaveFile()
    {
        SaveFileInner();
    }

    /// <summary>
    ///     另存为文件
    /// </summary>
    [RelayCommand]
    public void SaveAsFile()
    {
        SaveFileInner(true);
    }

    private void SaveFileInner(bool isSaveAs = false)
    {
        if (isSaveAs) _filePath = string.Empty;

        if (string.IsNullOrEmpty(_filePath))
            _messenger.Send(new MbFileDialog
            {
                WindowName = MAIN_WINDOW,
                IsOpenMode = false,
                FileFilterName = "Tactix Source",
                FileFilter = "*.tactixSource",
                Trigger = TACEDIT_TRIGGER
            });
        else
            SaveFileHandle(TACEDIT_TRIGGER);
    }

    /// <summary>
    ///     插入模板内容
    /// </summary>
    [RelayCommand]
    public void InsertTemplate()
    {
        TextDocument.Text += File.Exists(TACTIC_TEMPLATE_NAME)
            ? File.ReadAllText(TACTIC_TEMPLATE_NAME)
            : string.Empty;
    }

    /// <summary>
    ///     导出战术文件至
    /// </summary>
    [RelayCommand]
    public void ExportTo()
    {
        string? suggestPath = null;

        try
        {
            using var mod = new ModPackage(_config.CurrentlyEnabledMod);
            if (mod.ModDesc != null)
                suggestPath = Path.Combine(
                    Environment.CurrentDirectory,
                    TACTICS_FOLDER,
                    mod.ModDesc.TacticsPath);
        }
        catch (Exception ex)
        {
            _logger.Error($"Get Suggest Path Error: {ex.Message}");
        }

        _messenger.Send(new MbFileDialog
        {
            WindowName = MAIN_WINDOW,
            IsOpenMode = false,
            Trigger = EXPORT_TRIGGER,
            FileFilterName = "TactiX Tactic Files",
            FileFilter = "*.tactix",
            SuggestStartLocation = suggestPath
        });
    }

    /// <summary>
    ///     导出战术文件到默认目录
    /// </summary>
    [RelayCommand]
    public void Export()
    {
        using var mod = new ModPackage(_config.CurrentlyEnabledMod);
        if (mod.ModDesc == null)
        {
            _messenger.Send(new MbToastPureText
            {
                Message = _localizationService.GetString("ModsManageViewSelectedModError"),
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });

            return;
        }

        var suggestPath = Path.Combine(
            Environment.CurrentDirectory,
            TACTICS_FOLDER,
            mod.ModDesc.TacticsPath);

        var tactix = _encoder.Decoder(TextDocument.Text.Split(Environment.NewLine));
        if (tactix == null)
        {
            _messenger.Send(new MbToastPureText
            {
                Message = _localizationService.GetString("EditorErrorFileCantConvert"),
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });

            return;
        }

        var txt = JsonConvert.SerializeObject(tactix, Formatting.Indented);
        var outPath = Path.Combine(suggestPath, Path.GetFileNameWithoutExtension(_filePath) + ".tactix");

        // 确保目录存在，如果不存在则自动创建
        var directory = Path.GetDirectoryName(outPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(outPath, txt);

        _messenger.Send(new MbToastPureText
        {
            Message = string.Format(
                _localizationService.GetString("EditorExportSuccessInfo"),
                $"{Environment.NewLine}{outPath}"),
            Title = _localizationService.GetString("EditorExportSuccessTitle"),
            Type = MbEnumToastType.Success
        });
    }

    #endregion

    #region 逻辑

    /// <summary>
    ///     文件保存过程
    /// </summary>
    private void SaveFileHandle(string trigger)
    {
        try
        {
            if (trigger == TACEDIT_TRIGGER)
            {
                File.WriteAllText(_filePath, TextDocument.Text);
            }
            else if (trigger == EXPORT_TRIGGER)
            {
                var tactix = _encoder.Decoder(TextDocument.Text.Split(Environment.NewLine));
                if (tactix == null)
                {
                    _messenger.Send(new MbToastPureText
                    {
                        Message = _localizationService.GetString("EditorErrorFileCantConvert"),
                        Title = _localizationService.GetString("ToastTitleError"),
                        Type = MbEnumToastType.Error
                    });

                    return;
                }

                var txt = JsonConvert.SerializeObject(tactix, Formatting.Indented);

                // 确保目录存在，如果不存在则自动创建
                var directory = Path.GetDirectoryName(_exportPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(_exportPath, txt);

                _messenger.Send(new MbToastPureText
                {
                    Message = string.Format(
                        _localizationService.GetString("EditorExportSuccessInfo"),
                        $"{Environment.NewLine}{_exportPath}"),
                    Title = _localizationService.GetString("EditorExportSuccessTitle"),
                    Type = MbEnumToastType.Success
                });
            }

            _messenger.Send(new MbWindowTitle
            {
                Title = $"{TITLE_HEADER}{_filePath}",
                WindowName = MAIN_WINDOW
            });
        }
        catch (Exception ex)
        {
            _messenger.Send(new MbToastPureText
            {
                Message = ex.Message,
                Type = MbEnumToastType.Error,
                Title = _localizationService.GetString("ToastTitleError")
            });
        }
    }

    /// <summary>
    ///     文件打开过程
    /// </summary>
    private void OpenFileHandle()
    {
        try
        {
            TextDocument.Text = File.ReadAllText(_filePath);

            _messenger.Send(new MbWindowTitle
            {
                Title = $"{TITLE_HEADER}{_filePath}",
                WindowName = MAIN_WINDOW
            });
        }
        catch (Exception ex)
        {
            _messenger.Send(new MbToastPureText
            {
                Message = ex.Message,
                Type = MbEnumToastType.Error,
                Title = _localizationService.GetString("ToastTitleError")
            });
        }
    }

    #endregion
}