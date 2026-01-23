using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Messaging;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Popup;

public partial class TacticPlayWindow : Window
    , IRecipient<MbWindowClose>, IRecipient<MbWindowPointerTrans>
{
    #region 变量/常量

    private const string WINDOW_NAME = "TacticPlayWindow";

    #endregion

    public TacticPlayWindow(IMessenger messenger, IosTools oSTools)
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
    }

#if DEBUG
#pragma warning disable CS8618
    public TacticPlayWindow()
#pragma warning restore CS8618
    {
        InitializeComponent();
    }
#endif

    public void Receive(MbWindowClose message)
    {
        if (!message.Name.Equals(WINDOW_NAME)) return;

        Close();
    }

    public void Receive(MbWindowPointerTrans message)
    {
        _oses.SetMouseTransport(message.Enable);
    }

    private void TacticPlayWindow_Opened(object? sender, EventArgs e)
    {
        var screen = Screens.ScreenFromWindow(this);
        if (screen == null) return;

        Position = new PixelPoint(Convert.ToInt32(screen.WorkingArea.Center.X - Width / 2), 0);

        OsesSetHandle();
    }

    private void TacticPlayWindow_Closed(object? sender, EventArgs e)
    {
        _messenger.Send(new MbWindowStatus
        {
            WindowStatus = MbWindowStatus.MbEnumWindowStatus.Normal
        });

        _oses.UnregisterAllHotkeys();
        _oses.SaveConfig();
    }

    private void TacticPlayWindow_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse) BeginMoveDrag(e);
    }

    private void TacticPlayWindow_PointerExited(object? sender, PointerEventArgs e)
    {
        TransparencyLevelHint = new List<WindowTransparencyLevel> { WindowTransparencyLevel.Transparent };
    }

    private void TacticPlayWindow_PointerEntered(object? sender, PointerEventArgs e)
    {
        TransparencyLevelHint = [];
    }

    /// <summary>
    ///     OSTools注册窗体句柄
    /// </summary>
    private void OsesSetHandle()
    {
        var platformHandle = TryGetPlatformHandle();
        if (platformHandle != null) _oses.SetTacticPlayingWindowHandle(platformHandle.Handle);
    }

    #region DI容器注入

    private readonly IMessenger _messenger;
    private readonly IoSes _oses;
    private readonly LConfig _config;

    #endregion
}