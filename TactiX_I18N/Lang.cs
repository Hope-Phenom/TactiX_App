using System.Globalization;

namespace TactiX_I18N;

public class Lang : ILang
{
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

    public ILanguage Language { get; }
}