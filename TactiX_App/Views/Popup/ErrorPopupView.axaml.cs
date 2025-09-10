using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using TactiX_App.ViewModels.Popup;
using TactiX_Exception;
using TactiX_I18N;

namespace TactiX_App.Views.Popup;

public partial class ErrorPopupView : Window
{
    private readonly ErrorPopupViewModel _model;

    public ErrorPopupView()
    {
        InitializeComponent();
        _model = (ErrorPopupViewModel)DataContext!;
        RegisterCloseCallback();
    }

    public ErrorPopupView(TactiXException exception)
    { 
        InitializeComponent();
        _model = (ErrorPopupViewModel)DataContext!;
        _model.ReportModel.Error_Code = (int)exception.ErrorCode;
        _model.ReportModel.Error_Desc = exception.ErrorDesc;
        RegisterCloseCallback();
    }

    public void RegisterCloseCallback()
    {
        _model.PropertyChanged += (sender, e) => 
        {
            if (e.PropertyName == nameof(_model.CanBeClosed) && _model.CanBeClosed)
            {
                Close();
            }
        };
    }
}