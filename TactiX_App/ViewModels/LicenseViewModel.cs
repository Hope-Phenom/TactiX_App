using System;
using System.IO;

using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

using TactiX_I18N;
using TactiX_Models;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels
{
    public partial class LicenseViewModel : ViewModelBase
    {
        private const string EULA_ResPath = "avares://TactiX_App/Assets/EULA.md";
        private const string PP_ResPath = "avares://TactiX_App/Assets/Privacy_Policy.md";

        public ILanguage Language { get; set; }
        private IOSTools OSTools { get; set; }

        public LConfig Config { get; private set; }
        public string EULA_Text { get; private set; }
        public string PP_Text { get; private set; }

        [ObservableProperty]
        public string markdownText;

        public LicenseViewModel(IOSTools oSTools, ILang lang) 
        {
            OSTools = oSTools;
            Language = lang.Language;
            Config = oSTools.OSes.GetConfig();

            EULA_Text = GetTextFromRes(EULA_ResPath);
            PP_Text = GetTextFromRes(PP_ResPath);

            MarkdownText = EULA_Text;
        }

        private string GetTextFromRes(string path)
        {
            using var stream = AssetLoader.Open(new Uri(path));
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
