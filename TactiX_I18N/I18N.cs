using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TactiX_I18N
{
    public class I18N
    {
        #region 单例模式
        private static readonly Lazy<I18N> _lazyInstance = new Lazy<I18N>(() => new I18N(), isThreadSafe: true);
        public static I18N Instance => _lazyInstance.Value;
        private I18N()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "en-US":
                case "en-GB":
                    //Language = new English();
                    //break;
                case "zh-TW":
                case "zh-HK":
                case "zh-Hant":
                default:
                    Language = new Chinese();
                    break;
            }
        }
        #endregion

        public ILanguage Language { get; private set; }
    }
}
