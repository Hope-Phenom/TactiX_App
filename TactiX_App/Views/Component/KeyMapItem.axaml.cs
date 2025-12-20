using System;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using TactiX_Localization;
using TactiX_Models;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Component;

public partial class KeyMapItem : UserControl
{
    private LHotkeyBinding? _hotkeyBinding;

    private LHotkeyBindingEnum _hotkeyBindingEnum;

#if DEBUG
#pragma warning disable CS8618
    public KeyMapItem()
    {
        InitializeComponent();
    }
#pragma warning restore CS8618
#endif

    public KeyMapItem(IosTools oSTools, ILocalizationService localizationService)
    {
        InitializeComponent();

        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        _localizationService = localizationService;

        Keys = [.. Enum.GetValues<Key>()];
        KeyModifiers = [.. Enum.GetValues<KeyModifiers>()];

        ComboBox_Keys.ItemsSource = Keys;
        ComboBox_KeyModifiers.ItemsSource = KeyModifiers;

        ComboBox_Keys.SelectionChanged += ComboBox_Keys_SelectionChanged;
        ComboBox_KeyModifiers.SelectionChanged += ComboBox_KeyModifiers_SelectionChanged;
    }

    public AvaloniaList<Key> Keys { get; }
    public AvaloniaList<KeyModifiers> KeyModifiers { get; }

    public LHotkeyBindingEnum HotkeyBindingEnum
    {
        get => _hotkeyBindingEnum;
        set
        {
            _hotkeyBindingEnum = value;
            _hotkeyBinding = _config.Hotkeys
                .Where(o => o.Hotkey == HotkeyBindingEnum)
                .First();

            Label_KeyName.Content = _hotkeyBindingEnum.ToString();
            ComboBox_Keys.SelectedItem = _hotkeyBinding.Key;
            ComboBox_KeyModifiers.SelectedItem = _hotkeyBinding.Modifiers;
        }
    }

    private void ComboBox_Keys_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_hotkeyBinding == null || ComboBox_Keys.SelectedValue == null) return;

        _hotkeyBinding.Key = (Key)ComboBox_Keys.SelectedValue;
        _oses.SaveConfig();
        UpdateStatus();
    }

    private void ComboBox_KeyModifiers_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_hotkeyBinding == null || ComboBox_KeyModifiers.SelectedValue == null) return;

        _hotkeyBinding.Modifiers = (KeyModifiers)ComboBox_KeyModifiers.SelectedValue;
        _oses.SaveConfig();
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (_hotkeyBinding == null) return;

        Label_Status.Content = _oses.IsHotkeyAvailable(_hotkeyBinding.Key, _hotkeyBinding.Modifiers)
            ? _localizationService.GetString("NormalTextAvailable")
            : _localizationService.GetString("NormalTextError");
    }

    #region DI容器注入

    private readonly ILocalizationService _localizationService;
    private readonly IoSes _oses;
    private readonly LConfig _config;

    #endregion
}