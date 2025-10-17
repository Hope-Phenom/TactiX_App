using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TactiX_App.ViewModels.Popup;
using TactiX_Exception;
using TactiX_I18N;
using TactiX_Models.MessageBus;

namespace TactiX_App.Views.Popup;

public partial class ErrorPopupView : Window, IRecipient<MB_WindowClose>
{
    private const string WINDOW_NAME = "ErrorPopupView";
    private readonly IMessenger _messenger;

    public ErrorPopupView(IMessenger messenger)
    {
        InitializeComponent();

        Closed += ErrorPopupView_Closed;

        _messenger = messenger;
        _messenger.RegisterAll(this);
    }

    private void ErrorPopupView_Closed(object? sender, EventArgs e)
    {
        _messenger?.UnregisterAll(this);
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public ErrorPopupView()        // 此构造函数仅用于保证窗体浏览正常
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    {
        InitializeComponent();
    }
#endif

    public void Receive(MB_WindowClose message)
    {
        if (message.Name.Equals(WINDOW_NAME))
        {
            Close();
        }
    }
}