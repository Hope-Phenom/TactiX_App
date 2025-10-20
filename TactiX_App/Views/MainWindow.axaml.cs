using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;
using SukiUI.Toasts;
using System;
using System.Diagnostics;
using TactiX_App.ViewModels;
using TactiX_App.ViewModels.Popup;
using TactiX_App.Views.Popup;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views;

public partial class MainWindow : SukiWindow,
    IRecipient<MB_ToastPureText>, IRecipient<MB_ToastVersion>, IRecipient<MB_OpenTacticPlayWindow>,
    IRecipient<MB_WindowStatus>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOSes _oses;
    private readonly IMessenger _messenger;
    private readonly L_Config _config;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Opened += MainWindow_Opened;
        Closed += MainWindow_Closed;

        _serviceProvider = serviceProvider;
        _oses = _serviceProvider.GetRequiredService<IOSTools>().OSes;
        _messenger = _serviceProvider.GetRequiredService<IMessenger>();
        _config = _oses.LoadConfig();

        _messenger.RegisterAll(this);
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public MainWindow()        // 此构造函数仅用于保证窗体浏览正常
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    {
        InitializeComponent();
    }
#endif

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        OsesSetHandle();

        _oses.RegisterHotkey(
            Avalonia.Input.Key.Left,
            Avalonia.Input.KeyModifiers.Alt,
            () =>
            {
                Process.GetCurrentProcess().Kill();
            });

        _oses.RegisterWndProcHookCallback(this);
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _oses.UnregisterAllHotkeys();
    }

    /// <summary>
    /// OSTools绑定窗体句柄
    /// </summary>
    private void OsesSetHandle()
    {
        var platformHandle = TryGetPlatformHandle();
        if (platformHandle != null)
        {
            _oses.SetMainWindowHandle(platformHandle.Handle);
        }
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

    public void Receive(MB_OpenTacticPlayWindow message)
    {
        var tacPlayWindow = _serviceProvider.GetRequiredService<TacticPlayWindow>();
        tacPlayWindow.DataContext = _serviceProvider.GetRequiredService<TacticPlayWindowModel>();

        WindowState = WindowState.Minimized;

        tacPlayWindow.ShowDialog(this);
    }

    public void Receive(MB_WindowStatus message)
    {
        switch (message.WindowStatus)
        {
            case MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Normal:
                WindowState = WindowState.Normal;
                break;
            case MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Minimized:
                WindowState = WindowState.Minimized;
                break;
            case MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Maximized:
                WindowState = WindowState.Maximized;
                break;
        }
    }
    #endregion
}
