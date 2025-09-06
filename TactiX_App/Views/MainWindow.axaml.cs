using System;
using System.Threading.Tasks;
using System.Diagnostics;

using Avalonia.Controls;

using TactiX_OS_Tools;
using TactiX_Network;

namespace TactiX_App.Views;

public partial class MainWindow : Window
{
    #region 变量
    private IOSes OSes;
    #endregion

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Initialized;
    }

    private void MainWindow_Initialized(object? sender, System.EventArgs e)
    {
        Init();
    }

    public void Init()
    {
        var popup = new ErrorPopup();
        popup.ShowDialog(this);
    }
}
