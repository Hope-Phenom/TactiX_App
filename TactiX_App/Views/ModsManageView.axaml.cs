using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TactiX_App.Views;

public partial class ModsManageView : UserControl
{
    public ModsManageView()
    {
        // 显式地清楚上下文避免触发类型转换
        DataContext = null;

        InitializeComponent();
    }
}