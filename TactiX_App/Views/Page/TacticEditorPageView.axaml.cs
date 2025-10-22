using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TactiX_App.Model;
namespace TactiX_App.Views.Page;

public partial class TacticEditorPageView : UserControl
{
    public TacticEditorPageView()
    {
        InitializeComponent();

        Editor.SyntaxHighlighting = new TacticHighlightingDefinition();
    }
}