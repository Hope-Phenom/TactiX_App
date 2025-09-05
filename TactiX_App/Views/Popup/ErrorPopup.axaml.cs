using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TactiX_App.ViewModels.Popup;
using Tactix_Exception;
using TactiX_I18N;

namespace TactiX_App;

public partial class ErrorPopup : Window
{
    private readonly ErrorPopupModel _model;

    public ErrorPopup()
    {
        InitializeComponent();
        _model = (ErrorPopupModel)DataContext!;
    }

    public ErrorPopup(TactiXException exception)
    { 
        InitializeComponent();
        _model = (ErrorPopupModel)DataContext!;
        _model.ErrorCode = (int)exception.ErrorCode;
        _model.ErrorDesc = exception.ErrorDesc;
    }
}