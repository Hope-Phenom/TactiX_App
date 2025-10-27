using Avalonia;
using Avalonia.Dialogs;
using Avalonia.Logging;
using Avalonia.Platform.Storage;
using AvaloniaEdit.Document;
using AvaloniaEdit.Highlighting;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
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
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page
{
    public partial class TacticEditorPageViewModel : ViewModelBase, IRecipient<MB_FileDialog>
    {
        #region DI容器注入
        private readonly ILanguage _language;
        private readonly IMessenger _messenger;
        private readonly ITactiXSourceEncoder _encoder;
        private readonly IOSes _oses;
        private readonly L_Config _config;
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
        public ILanguage Language => _language;
        /// <summary>
        /// 编辑器中的文本
        /// </summary>
        public TextDocument TextDocument { get; private set; }
        #endregion

        public TacticEditorPageViewModel(IMessenger messenger, ILang lang, ITactiXSourceEncoder encoder,
            IOSTools oSTools, ILoggerContainer loggerContainer)
        {
            _language = lang.Language;
            _messenger = messenger;
            _encoder = encoder;
            _oses = oSTools.OSes;
            _config = _oses.LoadConfig();
            _logger = loggerContainer.Builder.GetCurrentClassLogger();

            _messenger.RegisterAll(this);

            TextDocument = new TextDocument();

            CreateNewFile();
        }


        #region UI响应的Commands
        /// <summary>
        /// 创建新文件
        /// </summary>
        [RelayCommand]
        public void CreateNewFile()
        {
            _filePath = string.Empty;

            TextDocument.Text = File.Exists(TACTIC_TEMPLATE_NAME)
                ? File.ReadAllText(TACTIC_TEMPLATE_NAME)
                : string.Empty;

            _messenger.Send(new MB_WindowTitle()
            {
                Title = $"{TITLE_HEADER}{NEW_FILE}",
                WindowName = MAIN_WINDOW
            });
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        [RelayCommand]
        public void OpenFile()
        {
            _messenger.Send(new MB_FileDialog()
            {
                WindowName = MAIN_WINDOW,
                IsOpenMode = true,
                FileFilterName = "TactiX Source",
                FileFilter = "*.tactixSource",
                Trigger = TACEDIT_TRIGGER
            });
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        [RelayCommand]
        public void SaveFile()
        {
            SaveFileInner();
        }
        /// <summary>
        /// 另存为文件
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
            {
                _messenger.Send(new MB_FileDialog()
                {
                    WindowName = MAIN_WINDOW,
                    IsOpenMode = false,
                    FileFilterName = "Tactix Source",
                    FileFilter = "*.tactixSource",
                    Trigger = TACEDIT_TRIGGER
                });
            }
            else
            {
                SaveFileHandle(TACEDIT_TRIGGER);
            }
        }
        /// <summary>
        /// 插入模板内容
        /// </summary>
        [RelayCommand]
        public void InsertTemplate()
        {
            TextDocument.Text += File.Exists(TACTIC_TEMPLATE_NAME)
                ? File.ReadAllText(TACTIC_TEMPLATE_NAME)
                : string.Empty;
        }
        /// <summary>
        /// 导出为战术文件
        /// </summary>
        [RelayCommand]
        public void Export()
        {
            string? suggestPath = null;

            try
            {
                using var mod = new ModPackage(_config.CurrentlyEnabledMOD);
                if (mod.ModDesc != null) suggestPath = Path.Combine(
                    Environment.CurrentDirectory,
                    TACTICS_FOLDER,
                    mod.ModDesc.TacticsPath);
            }
            catch (Exception ex)
            {
                _logger.Error($"Get Suggest Path Error: {ex.Message}");
            }

            _messenger.Send(new MB_FileDialog()
            {
                WindowName = MAIN_WINDOW,
                IsOpenMode = false,
                Trigger = EXPORT_TRIGGER,
                FileFilterName = "TactiX Tactic Files",
                FileFilter = "*.tactix",
                SuggestStartLocation = suggestPath
            });
        }
        #endregion

        #region 消息处理
        public void Receive(MB_FileDialog message)
        {
            if (string.IsNullOrEmpty(message.FilePath)) return;
            if (message.WindowName != MAIN_WINDOW) return;
            if (message.Trigger != EXPORT_TRIGGER 
                && message.Trigger != TACEDIT_TRIGGER) return;

            if (message.Trigger == EXPORT_TRIGGER)
            {
                _exportPath = message.FilePath;
            }
            else if (message.Trigger == TACEDIT_TRIGGER)
            {
                _filePath = message.FilePath;
            }

            if (message.IsOpenMode)
            {
                OpenFileHandle();
            }
            else
            {
                SaveFileHandle(message.Trigger);
            }
        }
        #endregion

        #region 逻辑
        /// <summary>
        /// 文件保存过程
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
                        _messenger.Send(new MB_ToastPureText()
                        {
                            Message = Language.EDITOR_ERROR_FILE_CANT_CONVERT,
                            Title = Language.TOAST_TITLE_ERROR,
                            Type = MB_Enum_ToastType.Error
                        });

                        return;
                    }

                    var txt = JsonConvert.SerializeObject(tactix, Formatting.Indented);
                    File.WriteAllText(_exportPath, txt);

                    _messenger.Send(new MB_ToastPureText()
                    {
                        Message = string.Format(
                            Language.EDITOR_EXPORT_SUCCESS_INFO, 
                            $"{Environment.NewLine}{_exportPath}"),
                        Title = Language.EDITOR_EXPORT_SUCCESS_TITLE,
                        Type = MB_Enum_ToastType.Success
                    });
                }

                _messenger.Send(new MB_WindowTitle()
                {
                    Title = $"{TITLE_HEADER}{_filePath}",
                    WindowName = MAIN_WINDOW
                });
            }
            catch (Exception ex)
            {
                _messenger.Send(new MB_ToastPureText()
                {
                    Message = ex.Message,
                    Type = MB_Enum_ToastType.Error,
                    Title = _language.TOAST_TITLE_ERROR
                });
            }
        }
        /// <summary>
        /// 文件打开过程
        /// </summary>
        private void OpenFileHandle()
        {
            try
            {
                TextDocument.Text = File.ReadAllText(_filePath);

                _messenger.Send(new MB_WindowTitle()
                {
                    Title = $"{TITLE_HEADER}{_filePath}",
                    WindowName = MAIN_WINDOW
                });
            }
            catch (Exception ex)
            {
                _messenger.Send(new MB_ToastPureText()
                {
                    Message = ex.Message,
                    Type = MB_Enum_ToastType.Error,
                    Title = _language.TOAST_TITLE_ERROR
                });
            }
        }
        #endregion
    }
}
