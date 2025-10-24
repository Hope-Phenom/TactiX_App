using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_App.Views;
using TactiX_I18N;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;

namespace TactiX_App.ViewModels.Page
{
    public partial class ReplayAnalysisPageViewModel : ViewModelBase, IRecipient<MB_FileDialog>
    {
        #region DI容器注入
        private readonly ILanguage _language;
        private readonly IReplayDecoder _replayDecoder;
        private readonly IMessenger _messenger;
        #endregion

        #region 数据绑定
        public ILanguage Language => _language;
        #endregion

        #region 变量/常量
        private const string MAIN_WINDOW = "MainWindow";
        private const string FILE_DIALOG_TRRIGER = "ReplayAnalysis";
        #endregion

        public ReplayAnalysisPageViewModel(ILang lang, IReplayDecoder replayDecoder, IMessenger messenger)
        {
            _language = lang.Language;
            _replayDecoder = replayDecoder;
            _messenger = messenger;

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

            HandleActionsDict(actionsDict);
        }
        #endregion

        #region 内部逻辑
        /// <summary>
        /// 处理解析的事件
        /// </summary>
        private void HandleActionsDict(Dictionary<string, List<L_ReplayAction>> dict)
        { 

        }
        #endregion
    }
}
