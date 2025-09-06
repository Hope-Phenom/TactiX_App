using Avalonia.Controls;

using TactiX_OS_Tools;
using TactiX_Network;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

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

        Task.Run(async () =>
        {
            var resp = await Network.Instance.Client.PostExceptionReportModel(new TactiX_Models.NExceptionReportModel()
            {
                Error_Code = 1,
                Error_Desc = "Refit测试",
                Feedback_Way = "C#",
                Feedback_Info = "Refit测试",
                Create_Time = DateTime.Now
            });
            Debug.WriteLine(resp.StatusCode);
            Debug.WriteLine(resp.Content);
        });
    }
}
