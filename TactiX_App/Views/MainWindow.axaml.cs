using System;

using Avalonia.Controls.Notifications;
using Avalonia.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using SukiUI.Controls;
using SukiUI.Toasts;

using TactiX_App.ViewModels;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views;

public partial class MainWindow : SukiWindow,
    IRecipient<MB_ToastPureText>, IRecipient<MB_ToastVersion>
{

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
    public void OsesSetHandle(IOSes OSes)
    {
        var platformHandle = TryGetPlatformHandle();
        if (platformHandle != null)
        {
            OSes.SetHandle(platformHandle.Handle);
        }
    }

    #region MessageBus 消息处理

    private NotificationType NotificationTypeConvert(MB_Enum_ToastType type)
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
            var vm = DataContext as MainViewModel;
            if (vm != null) vm.OSTools.OSes.OpenWeb(msg.Release_Url);
        }
    }

    #endregion
}
