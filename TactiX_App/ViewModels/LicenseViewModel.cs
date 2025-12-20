using System;
using System.Diagnostics;
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

namespace TactiX_App.ViewModels;

public partial class LicenseViewModel : ViewModelBase
{
    private const string EULA_RES_PATH = "avares://TactiX_App/Assets/EULA.md";
    private const string PP_RES_PATH = "avares://TactiX_App/Assets/Privacy_Policy.md";
    private readonly LConfig _config;
    private readonly ILogger _logger;

    private readonly INavigationService _navigationService;
    private readonly IosTools _oSTools;

    [ObservableProperty] public string markdownText;

    public LicenseViewModel(IosTools oSTools, ILang lang, INavigationService navigation, ILoggerContainer logger)
    {
        Language = lang.Language;
        EulaText = GetTextFromRes(EULA_RES_PATH);
        PpText = GetTextFromRes(PP_RES_PATH);

        _oSTools = oSTools;
        _navigationService = navigation;
        _logger = logger.Builder.GetCurrentClassLogger();
        _config = oSTools.OSes.LoadConfig();

        MarkdownText = EulaText;
        Status = 0;
    }

    public ILanguage Language { get; private set; }
    public string EulaText { get; }
    public string PpText { get; }

    /// <summary>
    ///     接受状态，0-未接受，1-只接受了EULA，2-全部接受
    /// </summary>
    private int Status { get; set; }

    private string GetTextFromRes(string path)
    {
        using var stream = AssetLoader.Open(new Uri(path));
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    [RelayCommand]
    public void Btn_Close()
    {
        Process.GetCurrentProcess().Kill();
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

            MarkdownText = PpText;
        }
        else
        {
            _config.PpAccepted = true;
            _oSTools.OSes.SaveConfig();
            _logger.Info("Agress PP.");

            _navigationService.NavigateTo<HomeScreenViewModel>();
            _logger.Info("Navi to HomeScreenView.");
        }
    }
}