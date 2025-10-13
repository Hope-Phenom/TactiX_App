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
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels
{
    public partial class ModsManageViewModel
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
        public string Info { get; private set; } = string.Empty;
        #endregion

        public ModsManageViewModel(ILang lang, ILoggerContainer loggerContainer, IMessenger messenger,
            IOSTools oSTools) 
        {
            Language = lang.Language;

            _logger = loggerContainer.Builder.GetCurrentClassLogger();
            _messenger = messenger;
            _oses = oSTools.OSes;

            CheckFolders();
            CheckLocalMods();
        }

        private void CheckFolders()
        {
            _oses.CheckOrCreateDir(TacitcsFolder);
            _oses.CheckOrCreateDir(ModsFolder);
        }

        [RelayCommand]
        public void CheckLocalMods()
        {
            var files = Directory.GetFiles(ModsFolder, "*.zip");
            Info = string.Format(Language.MODS_MANAGE_VIEW_LOCAL_MODS_INFO, files.Length);
        }

        [RelayCommand]
        public void NaviBack()
        {
            _messenger.Send(new MB_NavigationBack());
        }

        [RelayCommand]
        public void OpenLocalFolder()
        {
            _oses.OpenWeb(ModsFolder);
        }
    }
}
