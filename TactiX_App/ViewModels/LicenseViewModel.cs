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
        public INavigationService NavigationService { get; private set; }
        public ILogger Logger { get; private set; }
        private IOSTools OSTools { get; set; }

        public LConfig Config { get; private set; }
        public string EULA_Text { get; private set; }
        public string PP_Text { get; private set; }

        /// <summary>
        /// 接受状态，0-未接受，1-只接受了EULA，2-全部接受
        /// </summary>
        private int Status { get; set; }

        [ObservableProperty]
        public string markdownText;

        public LicenseViewModel(IOSTools oSTools, ILang lang, INavigationService navigation, ILoggerContainer logger) 
        {
            OSTools = oSTools;
            Language = lang.Language;
            NavigationService = navigation;
            Logger = logger.Builder.GetCurrentClassLogger();
            Config = oSTools.OSes.LoadConfig();

            EULA_Text = GetTextFromRes(EULA_ResPath);
            PP_Text = GetTextFromRes(PP_ResPath);

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
            Logger.Warn("DECLINE & EXIT.");
        }

        [RelayCommand]
        public void Btn_Agress()
        {
            Status++;

            if (Status == 1)
            {
                MarkdownText = PP_Text;
                Logger.Info("Agress EULA.");
            }
            else
            {
                Logger.Info("Agress PP.");

                Config.EulaAccepted = true;
                OSTools.OSes.SaveConfig();

                NavigationService.NavigateTo<HomeScreenViewModel>();
                Logger.Info("Navi to HomeScreenView.");
            }
        }
    }
}
