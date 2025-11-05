using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TactiX_App.Views;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;

namespace TactiX_App.ViewModels.Page
{
    public partial class ReplayAnalysisPageViewModel : ViewModelBase, 
        IRecipient<MB_FileDialog>, IRecipient<MB_FolderDialog>
    {
        #region DI容器注入
        private readonly ILanguage _language;
        private readonly IReplayDecoder _replayDecoder;
        private readonly IMessenger _messenger;
        private readonly ILogger _logger;
        #endregion

        #region 数据绑定
        public ILanguage Language => _language;
        #endregion

        #region 变量/常量
        private const string MAIN_WINDOW = "MainWindow";
        private const string FILE_DIALOG_TRRIGER = "ReplayAnalysis";
        private const string FOLDER_DIALOG_TRRIGER = "ReplayAnalysisFolder";
        private const string TEMPLATE_PATH = "TacticTemplate.tactixSource";
        private const string EXPORT_FILE_NAME = "{0}_{1}.tactixSource";

        private Dictionary<string, List<L_ReplayAction>>? _actionsDict;
        private string? _folderPath;
        #endregion

        public ReplayAnalysisPageViewModel(ILang lang, IReplayDecoder replayDecoder, IMessenger messenger,
            ILoggerContainer loggerContainer)
        {
            _language = lang.Language;
            _replayDecoder = replayDecoder;
            _messenger = messenger;
            _logger = loggerContainer.Builder.GetCurrentClassLogger();

            _messenger.RegisterAll(this);
        }

        #region Command绑定
        /// <summary>
        /// 解析星际2的回放文件
        /// </summary>
        [RelayCommand]
        public void DecodeSC2Replay()
        {
            _messenger.Send(new MB_FileDialog()
            {
                WindowName = MAIN_WINDOW,
                IsOpenMode = true,
                Trigger = FILE_DIALOG_TRRIGER,
                FileFilterName = "StarCraft II Replays",
                FileFilter = "*.SC2Replay"
            });
        }
        #endregion

        #region MB消息处理
        public async void Receive(MB_FileDialog message)
        {
            if (message.Trigger != FILE_DIALOG_TRRIGER) return;
            if (string.IsNullOrEmpty(message.FilePath)) return;

            var filePath = message.FilePath;
            var actionsDict = await _replayDecoder.DecodeReplay(filePath);
            if (actionsDict.Count == 0) return;

            _actionsDict = actionsDict;

            _messenger.Send(new MB_FolderDialog()
            { 
                Trigger = FOLDER_DIALOG_TRRIGER,
                WindowName = MAIN_WINDOW
            });
        }

        public void Receive(MB_FolderDialog message)
        {
            if (message.Trigger != FOLDER_DIALOG_TRRIGER) return;
            if (string.IsNullOrEmpty(message.FolderPath)) return;

            _folderPath = message.FolderPath;

            HandleActionsDict();
        }
        #endregion

        #region 内部逻辑
        /// <summary>
        /// 处理解析的事件
        /// </summary>
        private void HandleActionsDict()
        {
            if (!File.Exists(TEMPLATE_PATH)) return;
            if (_actionsDict == null || _folderPath == null) return;

            var dateTime = DateTime.Now;

            try
            {
                var exportPaths = Environment.NewLine;

                foreach (var keyValue in _actionsDict)
                {
                    var fileName = string.Format(EXPORT_FILE_NAME,
                        keyValue.Key,
                        dateTime.ToString("yyyy_MM_dd_HH_mm_ss"));
                    var filePath = Path.Combine(_folderPath, fileName);
                    var sb = new StringBuilder(File.ReadAllText(TEMPLATE_PATH));

                    sb.AppendLine(Environment.NewLine);
                    for (int i = 1; i <= keyValue.Value.Count; i++)
                    {
                        var act = keyValue.Value[i - 1];
                        if (act == null) continue;

                        sb.AppendLine($"- {i}, {SecondsToMmSs(act.Time)}, {act.Abbr}, {act.Supply}");
                    }

                    File.WriteAllText(filePath, sb.ToString());

                    exportPaths += filePath + ";" + Environment.NewLine;
                }

                _messenger.Send(new MB_ToastPureText()
                {
                    Message = string.Format(Language.REPLAY_ANALYSIS_DECODE_SUCCESS, exportPaths),
                    Title = Language.REPLAY_ANALYSIS_DECODE_SUCCESS_TITLE,
                    Type = MB_Enum_ToastType.Success
                });
            }
            catch (Exception ex)
            {
                _messenger.Send(new MB_ToastPureText()
                { 
                    Message = string.Format(Language.REPLAY_ANALYSIS_DECODE_ERROR, ex.Message),
                    Title = Language.REPLAY_ANALYSIS_DECODE_ERROR_TITLE,
                    Type = MB_Enum_ToastType.Error
                });

                _logger.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 将总秒数转换为 mm:ss 格式的字符串（自动补零）
        /// </summary>
        /// <param name="totalSeconds">总秒数（非负整数）</param>
        /// <returns>格式化后的时间字符串（如 05:30）</returns>
        private static string SecondsToMmSs(int totalSeconds)
        {
            // 确保秒数非负
            if (totalSeconds < 0)
            {
                throw new ArgumentException("秒数不能为负数", nameof(totalSeconds));
            }

            // 计算分钟和剩余秒数
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            // 格式化为 mm:ss（自动补零）
            return $"{minutes:D2}:{seconds:D2}";
        }
        #endregion
    }
}
