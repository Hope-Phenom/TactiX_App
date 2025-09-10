using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TactiX_App.ViewModels;
using TactiX_OS_Tools;

namespace TactiX_App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += MainWindow_Opened;
        PointerPressed += MainWindow_PointerPressed;
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        var vm = MainView.DataContext as MainViewModel;
        if (vm != null) 
        {
            OsesSetHandle(vm.OSTools.OSes);
        }
    }

    private void MainWindow_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Pointer.Type == PointerType.Mouse)
        {
            BeginMoveDrag(e);
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
}
