using TactiX_I18N;

namespace TactiX_Exception;

/// <summary>
///     TactiX 内部异常
/// </summary>
[Serializable]
public class TactiXException : Exception
{
    public TactiXException(ILang lang)
    {
        ErrorDesc = string.Empty;
        Language = lang.Language;
    }

    /// <summary>
    ///     传参构建异常
    /// </summary>
    /// <param name="errorCode">错误码</param>
    /// <param name="errorDesc">错误信息</param>
    public TactiXException(ILang lang, TactiXErrorCodes errorCode, string errorDesc)
    {
        ErrorCode = errorCode;
        ErrorDesc = errorDesc;
        Language = lang.Language;
    }

    public ILanguage Language { get; private set; }

    /// <summary>
    ///     错误码
    /// </summary>
    public TactiXErrorCodes ErrorCode { get; private set; }

    /// <summary>
    ///     错误描述文本
    /// </summary>
    public string ErrorDesc { get; private set; }

    public override string ToString()
    {
        return Language.ErrorDescTemplate
            .Replace("{0}", $"{ErrorCode}")
            .Replace("{1}", ErrorDesc);
    }
}