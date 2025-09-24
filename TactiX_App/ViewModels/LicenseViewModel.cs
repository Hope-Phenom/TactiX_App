using System;
using System.IO;

using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;

using TactiX_App.Service;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels
{
    public partial class LicenseViewModel : ViewModelBase
    {
        private const string EULA_ResPath = "avares://TactiX_App/Assets/EULA.md";
        private const string PP_ResPath = "avares://TactiX_App/Assets/Privacy_Policy.md";

        public ILanguage Language { get; private set; }
        public string EULA_Text { get; private set; }
        public string PP_Text { get; private set; }

        private readonly INavigationService _navigationService;
        private readonly ILogger _logger;
        private readonly IOSTools _oSTools;
        private readonly L_Config _config;

        /// <summary>
        /// 接受状态，0-未接受，1-只接受了EULA，2-全部接受
        /// </summary>
        private int Status { get; set; }

        [ObservableProperty]
        public string markdownText;

        public LicenseViewModel(IOSTools oSTools, ILang lang, INavigationService navigation, ILoggerContainer logger) 
        {
            Language = lang.Language;
            EULA_Text = GetTextFromRes(EULA_ResPath);
            PP_Text = GetTextFromRes(PP_ResPath);

            _oSTools = oSTools;
            _navigationService = navigation;
            _logger = logger.Builder.GetCurrentClassLogger();
            _config = oSTools.OSes.LoadConfig();

            MarkdownText = EULA_Text;
            Status = 0;
        }

        private string GetTextFromRes(string path)
        {
            using var stream = AssetLoader.Open(new Uri(path));
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        [RelayCommand]
        public void Btn_Close()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            _logger.Warn("DECLINE & EXIT.");
        }

        [RelayCommand]
        public void Btn_Agress()
        {
            Status++;

            if (Status == 1)
            {
                _config.EulaAccepted = true;
                _oSTools.OSes.SaveConfig();
                _logger.Info("Agress EULA.");

                MarkdownText = PP_Text;
            }
            else
            {
                _config.PPAccepted = true;
                _oSTools.OSes.SaveConfig();
                _logger.Info("Agress PP.");

                _navigationService.NavigateTo<HomeScreenViewModel>();
                _logger.Info("Navi to HomeScreenView.");
            }
        }
    }
}
