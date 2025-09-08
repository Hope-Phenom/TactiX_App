using CommunityToolkit.Mvvm.ComponentModel;
using TactiX_I18N;

namespace TactiX_App.ViewModels;

public class ViewModelBase : ObservableObject
{
    public ILanguage Language => I18N.Instance.Language;
}
