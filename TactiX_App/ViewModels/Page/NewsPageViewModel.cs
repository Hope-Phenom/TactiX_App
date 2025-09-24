using Avalonia.Controls;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_App.Service;
using TactiX_App.Views;
using TactiX_I18N;

namespace TactiX_App.ViewModels.Page
{
    public class NewsPageViewModel : PageViewModelBase
    {
        private readonly ILanguage _language;

        public NewsPageViewModel(ILang lang, IServiceProvider serviceProvider)
        {
            _language = lang.Language;

            Index = 0;
            DisplayName = _language.HOMESCREEN_SIDE_NEWS;
            Icon = MaterialIconKind.Home;
        }
    }
}
