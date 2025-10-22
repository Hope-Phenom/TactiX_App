using AvaloniaEdit.Document;
using AvaloniaEdit.Highlighting;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_I18N;

namespace TactiX_App.ViewModels.Page
{
    public partial class TacticEditorPageViewModel : ViewModelBase
    {
        #region DI容器注入
        private readonly ILanguage _language;
        private readonly IMessenger _messenger;
        #endregion

        #region 常量
        private const string TACTIC_TEMPLATE_NAME = "TacticTemplate.tactixSource";
        #endregion

        #region 数据绑定
        public ILanguage Language => _language;
        /// <summary>
        /// 编辑器中的文本
        /// </summary>
        public TextDocument TextDocument { get; private set; }
        #endregion

        public TacticEditorPageViewModel(IMessenger messenger, ILang lang)
        {
            _language = lang.Language;
            _messenger = messenger;

            TextDocument = new TextDocument();
            TextDocument.Text = File.Exists(TACTIC_TEMPLATE_NAME)
                ? File.ReadAllText(TACTIC_TEMPLATE_NAME)
                : string.Empty;
        }
    }
}
