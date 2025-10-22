using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_I18N;

namespace TactiX_App.ViewModels.Page
{
    public class TacticEditorPageViewModel : ViewModelBase
    {
        #region DI容器注入
        private readonly ILanguage _language;
        private readonly IMessenger _messenger;
        #endregion

        #region 数据绑定
        public ILanguage Language => _language;
        #endregion

        public TacticEditorPageViewModel(IMessenger messenger, ILang lang)
        {
            _language = lang.Language;
            _messenger = messenger;
        }
    }
}
