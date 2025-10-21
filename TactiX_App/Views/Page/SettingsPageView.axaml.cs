using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using TactiX_App.Views.Component;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Page;

public partial class SettingsPageView : UserControl, IRecipient<MB_FIX_SettingsLayoutItemsHeader>
{
    #region DI容器注入
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IOSes _oses;
    private readonly L_Config _config;
    #endregion

    public SettingsPageView(IMessenger messenger, IOSTools oSTools, IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        _messenger = messenger;
        _messenger.RegisterAll(this);

        AttachedToVisualTree += SettingsPageView_AttachedToVisualTree;
    }

    private void SettingsPageView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        UpdateKeyMap();
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public SettingsPageView() { } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

    public void UpdateKeyMap()
    {
        if (StackPanel_KeyMaps.Children.Count >= _config.Hotkeys.Length) return;

        foreach (var hotkey in _config.Hotkeys)
        {
            var keymap = _serviceProvider.GetRequiredService<KeyMapItem>();
            keymap.HotkeyBindingEnum = hotkey.Hotkey;

            StackPanel_KeyMaps.Children.Add(keymap);
            StackPanel_KeyMaps.Children.Add(new Grid() { Height = 10 });
        }
    }

    public void Receive(MB_FIX_SettingsLayoutItemsHeader message)
    {
        switch (message.Name)
        {
            case "Normal":
                SettingsLayoutItem_Normal.Header = message.HeaderText;
                break;
            case "Hotkey":
                SettingsLayoutItem_Hotkey.Header = message.HeaderText;
                break;
            case "About":
                SettingsLayoutItem_About.Header = message.HeaderText;
                break;
        }
    }
}