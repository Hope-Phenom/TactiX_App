using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using HarfBuzzSharp;
using System;
using TactiX_Models.MessageBus;

namespace TactiX_App.Views.Popup;

public partial class TacticPlayWindow : Window
{
    #region DI容器注入
    private readonly IMessenger _messenger;
    #endregion

    public TacticPlayWindow(IMessenger messenger)
    {
        InitializeComponent();

        Closed += TacticPlayWindow_Closed;
        Opened += TacticPlayWindow_Opened;

        _messenger = messenger;
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public TacticPlayWindow()  // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    {
        InitializeComponent();
    }
#endif

    private void TacticPlayWindow_Opened(object? sender, EventArgs e)
    {
        var screen = Screens.ScreenFromWindow(this);
        if (screen == null) return;

        Position = new PixelPoint(Convert.ToInt32(screen.WorkingArea.Center.X - Width / 2), 0);
    }

    private void TacticPlayWindow_Closed(object? sender, EventArgs e)
    {
        _messenger.Send(new MB_WindowStatus() 
        {
            WindowStatus = MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Normal
        });
    }
}