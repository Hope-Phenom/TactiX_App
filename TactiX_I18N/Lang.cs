using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TactiX_I18N
{
    public class Lang : ILang
    {
        public ILanguage Language { get; private set; }

        public Lang()
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
    }
}
