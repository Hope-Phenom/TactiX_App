using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_I18N;
using TactiX_Logger;

namespace TactiX_App.ViewModels.Page
{
    public class TacticsHallPageViewModel: ViewModelBase
    {
        #region DI容器注入
        public ILanguage Language { get; set; }
        private readonly Logger _logger;
        #endregion

        public TacticsHallPageViewModel(ILang lang, ILoggerContainer loggerContainer)
        {
            Language = lang.Language;
            _logger = loggerContainer.Builder.GetCurrentClassLogger();
        }
    }
}
