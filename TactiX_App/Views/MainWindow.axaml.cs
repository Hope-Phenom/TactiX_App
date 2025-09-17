using System;

using Avalonia.Input;
using CommunityToolkit.Mvvm.Messaging;
using SukiUI.Controls;

using TactiX_App.ViewModels;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views;

public partial class MainWindow : SukiWindow, IRecipient<MB_ToastPureText>, IRecipient<MB_ToastVersion>
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
        var vm = MainViewContainer.DataContext as MainViewModel;
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

    public void Receive(MB_ToastPureText msg)
    {
        System.Diagnostics.Debug.WriteLine(msg);
    }

    public void Receive(MB_ToastVersion msg)
    {
        System.Diagnostics.Debug.WriteLine(msg);
    }

    #endregion
}
