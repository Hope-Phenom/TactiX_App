using System;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using SukiUI.Controls;
using SukiUI.Toasts;

using TactiX_App.ViewModels;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views;

public partial class MainWindow : SukiWindow,
    IRecipient<MB_ToastPureText>, IRecipient<MB_ToastVersion>, IRecipient<MB_NavigationTo>
{

    #region 记录窗体状态
    private Point _size = new();
    private bool _isMax;
    #endregion

    public MainWindow()
    {
        InitializeComponent();

        Opened += MainWindow_Opened;

        // 注册消息接收
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        var vm = DataContext as MainViewModel;
        if (vm != null) 
        {
            OsesSetHandle(vm.OSTools.OSes);
        }
    }

    /// <summary>
    /// OSTools绑定窗体句柄
    /// </summary>
    private void OsesSetHandle(IOSes OSes)
    {
        var platformHandle = TryGetPlatformHandle();
        if (platformHandle != null)
        {
            OSes.SetHandle(platformHandle.Handle);
        }
    }

    /// <summary>
    /// 将窗体设置为战术播放模式（不再像旧版弹出新窗体）
    /// </summary>
    private void SetWindowToTacticPlayMode()
    {

    }

    #region MessageBus 消息处理

    /// <summary>
    /// 枚举转换
    /// </summary>
    private static NotificationType NotificationTypeConvert(MB_Enum_ToastType type)
    {
        NotificationType _type = type switch
        {
            MB_Enum_ToastType.Info => NotificationType.Information,
            MB_Enum_ToastType.Success => NotificationType.Success,
            MB_Enum_ToastType.Warn => NotificationType.Warning,
            MB_Enum_ToastType.Error => NotificationType.Error,
            _ => NotificationType.Information
        };

        return _type;
    }

    public void Receive(MB_ToastPureText msg)
    {
        Dispatcher.UIThread.InvokeAsync(new Action(() =>
        {
            ToastHost.Manager.CreateToast()
                .OfType(NotificationTypeConvert(msg.Type))
                .Dismiss().After(TimeSpan.FromSeconds(30))
                .Dismiss().ByClicking()
                .WithTitle(msg.Title)
                .WithContent(msg.Message)
                .Queue();
        }));
    }

    public void Receive(MB_ToastVersion msg)
    {
        Dispatcher.UIThread.InvokeAsync(new Action(() =>
        {
            ToastHost.Manager.CreateToast()
                .OfType(NotificationTypeConvert(msg.Type))
                .Dismiss().After(TimeSpan.FromSeconds(30))
                .Dismiss().ByClicking()
                .WithTitle(msg.Title)
                .WithContent(msg.Message)
                .Queue();
        }));

        if (!string.IsNullOrEmpty(msg.Release_Url))
        {
            if (DataContext is MainViewModel vm)
            { 
                vm.OSTools.OSes.OpenUrl(msg.Release_Url);
            }
        }
    }

    public void Receive(MB_NavigationTo message)
    {
        switch (message.WindowsStatus)
        {
            case 0:
                Width = _size.X;
                Height = _size.Y;
                WindowState = _isMax
                    ? Avalonia.Controls.WindowState.Maximized
                    : Avalonia.Controls.WindowState.Normal;
                break;
            case 1:
                _size = new Point(Width, Height);
                _isMax = WindowState == Avalonia.Controls.WindowState.Maximized;
                Width = 200;
                Height = 200;
                break;
            case -1:
            default:
                break;
        }
    }
    #endregion
}
