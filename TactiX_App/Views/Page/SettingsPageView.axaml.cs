using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using TactiX_Models.MessageBus;

namespace TactiX_App.Views.Page;

public partial class SettingsPageView : UserControl, IRecipient<MB_FIX_SettingsLayoutItemsHeader>
{
    #region DI容器注入
    private readonly IMessenger _messenger;
    #endregion

    public SettingsPageView(IMessenger messenger)
    {
        InitializeComponent();

        _messenger = messenger;
        _messenger.RegisterAll(this);
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public SettingsPageView() { } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
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
}