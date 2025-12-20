using System;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using TactiX_App.Views.Component;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Page;

public partial class SettingsPageView : UserControl, IRecipient<MB_FIX_SettingsLayoutItemsHeader>
{
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

#if DEBUG
#pragma warning disable CS8618 // ���˳����캯��ʱ������Ϊ null ���ֶα�������� null ֵ���뿼������ "required" ���η�������Ϊ��Ϊ null��
    public SettingsPageView() // �˹��캯�������ڱ�֤��Ԥ��
    {
        InitializeComponent();
    }
#pragma warning restore CS8618 // ���˳����캯��ʱ������Ϊ null ���ֶα�������� null ֵ���뿼������ "required" ���η�������Ϊ��Ϊ null��
#endif

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

    private void SettingsPageView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        UpdateKeyMap();
    }

    public void UpdateKeyMap()
    {
        if (StackPanel_KeyMaps.Children.Count >= _config.Hotkeys.Length) return;

        foreach (var hotkey in _config.Hotkeys)
        {
            var keymap = _serviceProvider.GetRequiredService<KeyMapItem>();
            keymap.HotkeyBindingEnum = hotkey.Hotkey;

            StackPanel_KeyMaps.Children.Add(keymap);
            StackPanel_KeyMaps.Children.Add(new Grid { Height = 10 });
        }
    }

    #region DI����ע��

    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IOSes _oses;
    private readonly L_Config _config;

    #endregion
}