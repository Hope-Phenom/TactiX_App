using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_App.Service;
using TactiX_I18N;

namespace TactiX_App.ViewModels
{
    public class HomeScreenViewModel : ViewModelBase
    {
        public ILanguage Language { get; private set; }
        public INavigationService NavigationService { get; private set; }

        public HomeScreenViewModel(ILang lang, INavigationService navigation) 
        {
            Language = lang.Language;
            NavigationService = navigation;
        }
    }
}
