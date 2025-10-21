using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Linq;
using TactiX_I18N;
using TactiX_Models;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Component;

public partial class KeyMapItem : UserControl
{
    #region DI容器注入
    private readonly IOSes _oses;
    private readonly L_Config _config;
    private readonly ILanguage _language;
    #endregion

    public AvaloniaList<Key> Keys { get; private set; }
    public AvaloniaList<KeyModifiers> KeyModifiers { get; private set; }

    private L_HotkeyBinding? _hotkeyBinding;

    private L_HotkeyBindingEnum _hotkeyBindingEnum;
    public L_HotkeyBindingEnum HotkeyBindingEnum 
    {
        get => _hotkeyBindingEnum;
        set
        {
            _hotkeyBindingEnum = value;
            _hotkeyBinding = _config.Hotkeys
                .Where(o => o.Hotkey == HotkeyBindingEnum)
                .First();

            Label_KeyName.Content =  _hotkeyBindingEnum.ToString();
            ComboBox_Keys.SelectedItem = _hotkeyBinding.Key;
            ComboBox_KeyModifiers.SelectedItem = _hotkeyBinding.Modifiers;
        }
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public KeyMapItem() // 此构造函数仅用于保证可预览
    {
        InitializeComponent();
    } 
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

    public KeyMapItem(IOSTools oSTools, ILang lang)
    {
        InitializeComponent();

        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        _language = lang.Language;

        Keys = [.. Enum.GetValues<Key>()];
        KeyModifiers = [.. Enum.GetValues<KeyModifiers>()];

        ComboBox_Keys.ItemsSource = Keys;
        ComboBox_KeyModifiers.ItemsSource = KeyModifiers;

        ComboBox_Keys.SelectionChanged += ComboBox_Keys_SelectionChanged;
        ComboBox_KeyModifiers.SelectionChanged += ComboBox_KeyModifiers_SelectionChanged;
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
            ? _language.NORMAL_TEXT_AVAILABLE
            : _language.NORMAL_TEXT_ERROR;
    }
}