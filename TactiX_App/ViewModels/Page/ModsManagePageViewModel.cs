using System;
using System.IO;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using TactiX_Localization;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page;

public partial class ModsManagePageViewModel : ViewModelBase
{
    public ModsManagePageViewModel(ILocalizationService localizationService, ILoggerContainer loggerContainer,
        IMessenger messenger,
        IosTools oSTools)
    {
        _localizationService = localizationService;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _messenger = messenger;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        CurrMod = _config.CurrentlyEnabledMod;

        ModsList = [];

        CheckFolders();
        CheckLocalMods();
    }

    private void CheckFolders()
    {
        _oses.CheckOrCreateDir(TACTICS_FOLDER);
        _oses.CheckOrCreateDir(MODS_FOLDER);
        _oses.LoadConfig();
    }

    [RelayCommand]
    private void CheckLocalMods()
    {
        var files = Directory.GetFiles(MODS_FOLDER, "*.zip");
        var len = files.Length;
        Info = string.Format(_localizationService.GetString("ModsManageViewLocalModsInfo"), len);

        if (len > 0)
        {
            ModsList.Clear();
            ModsList.AddRange(files);
        }

        CurrMod = _config.CurrentlyEnabledMod;
        SelectedModIcon = null;
    }

    [RelayCommand]
    private void NaviBack()
    {
        _messenger.Send(new MbNavigationBack());
    }

    [RelayCommand]
    private void OpenLocalFolder()
    {
        _oses.OpenUrl(MODS_FOLDER);
    }

    private void OnSelectionChanged()
    {
        try
        {
            if (string.IsNullOrEmpty(SelectedItem)) return;

            SelectedModIcon?.Dispose();

            using var mod = new ModPackage(SelectedItem);
            using var ms = new MemoryStream(mod.ReadBinaryFile(ICON_PATH));

            ModDesc = mod.ModDesc;
            SelectedModIcon = new Bitmap(ms);
        }
        catch (Exception ex)
        {
            var errMsg = string.Format(_localizationService.GetString("ModsManageViewSelectedModError")
                , SelectedItem,
                ex.Message);

            _messenger.Send(new MbToastPureText
            {
                Message = errMsg,
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });

            _logger.Error(errMsg);
        }
    }

    /// <summary>
    ///     启用选定的MOD
    /// </summary>
    [RelayCommand]
    private void EnableSelectedMod()
    {
        if (string.IsNullOrEmpty(SelectedItem)) return;

        _config.CurrentlyEnabledMod = SelectedItem;
        _oses.SaveConfig();

        CheckLocalMods();
    }

    /// <summary>
    ///     删除选定的MOD
    /// </summary>
    [RelayCommand]
    private void DeleteSelectedMod()
    {
        if (string.IsNullOrEmpty(SelectedItem)) return;

        try
        {
            File.Delete(SelectedItem);
            CheckLocalMods();
        }
        catch (Exception ex)
        {
            var errMsg = string.Format(
                _localizationService.GetString("ModsManageViewModActionDeleteSelectedError"),
                SelectedItem,
                ex.Message);

            _messenger.Send(new MbToastPureText
            {
                Message = errMsg,
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });

            _logger.Error(errMsg);
        }
    }

    #region DI注入

    private readonly ILocalizationService _localizationService;
    private readonly Logger _logger;
    private readonly IMessenger _messenger;
    private readonly IoSes _oses;

    #endregion

    #region 常量/变量

    private const string TACTICS_FOLDER = "Tactics";
    private const string MODS_FOLDER = "Mods";
    private const string ICON_PATH = "manifest.png";

    /// <summary>
    ///     UI信息显示
    /// </summary>
    [ObservableProperty] private string _info = string.Empty;

    /// <summary>
    ///     Mods列表
    /// </summary>
    public AvaloniaList<string> ModsList { get; }

    #region Listbox 数据绑定

    private string? _selectedItem;

    public string? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnSelectionChanged();
            }
        }
    }

    #endregion

    /// <summary>
    ///     当前展示的MOD信息
    /// </summary>
    [ObservableProperty] private LModDesc? _modDesc;

    /// <summary>
    ///     配置，用于读取当前选定的MOD
    /// </summary>
    private readonly LConfig _config;

    /// <summary>
    ///     当前启用的MOD
    /// </summary>
    [ObservableProperty] private string _currMod;

    [ObservableProperty] private Bitmap? _selectedModIcon;

    #endregion
}