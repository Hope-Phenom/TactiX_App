using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Messaging;
using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using TactiX_I18N;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Popup;

public partial class TacticPlayWindow : Window
    , IRecipient<MB_WindowClose>, IRecipient<MB_WindowPointerTrans>
{
    #region DI容器注入
    private readonly IMessenger _messenger;
    private readonly IOSes _oses;
    private readonly L_Config _config;
    #endregion

    #region 变量/常量
    private const string WINDOW_NAME = "TacticPlayWindow";
    #endregion

    public TacticPlayWindow(IMessenger messenger, IOSTools oSTools, ILang lang)
    {
        InitializeComponent();

        Closed += TacticPlayWindow_Closed;
        Opened += TacticPlayWindow_Opened;
        PointerPressed += TacticPlayWindow_PointerPressed;
        PointerEntered += TacticPlayWindow_PointerEntered;
        PointerExited += TacticPlayWindow_PointerExited;

        _messenger = messenger;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();

        _messenger.RegisterAll(this);

        RequestedThemeVariant = _config.NightMode
            ? ThemeVariant.Dark
            : ThemeVariant.Light;
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

        OsesSetHandle();
    }

    private void TacticPlayWindow_Closed(object? sender, EventArgs e)
    {
        _messenger.Send(new MB_WindowStatus() 
        {
            WindowStatus = MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Normal
        });
    }

    private void TacticPlayWindow_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            BeginMoveDrag(e);
        }
    }

    private void TacticPlayWindow_PointerExited(object? sender, PointerEventArgs e)
    {
        TransparencyLevelHint = new List<WindowTransparencyLevel>() { WindowTransparencyLevel.Transparent };
        Opacity = _config.Opacity;
    }

    private void TacticPlayWindow_PointerEntered(object? sender, PointerEventArgs e)
    {
        TransparencyLevelHint = [];
        Opacity = 1;
    }

    /// <summary>
    /// OSTools绑定窗体句柄
    /// </summary>
    private void OsesSetHandle()
    {
        var platformHandle = TryGetPlatformHandle();
        if (platformHandle != null)
        {
            _oses.SetTacticPlayingWindowHandle(platformHandle.Handle);
        }
    }

    public void Receive(MB_WindowClose message)
    {
        if (!message.Name.Equals(WINDOW_NAME)) return;

        Close();
    }

    public void Receive(MB_WindowPointerTrans message)
    {
        _oses.SetMouseTransport(message.Enable);
    }
}