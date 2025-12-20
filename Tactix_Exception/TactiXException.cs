using TactiX_Localization;

namespace TactiX_Exception;

/// <summary>
///     TactiX 内部异常
/// </summary>
[Serializable]
public class TactiXException : Exception
{
    public TactiXException(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        ErrorDesc = string.Empty;
    }

    /// <summary>
    ///     传参构建异常
    /// </summary>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="errorCode">错误码</param>
    /// <param name="errorDesc">错误信息</param>
    public TactiXException(ILocalizationService localizationService, TactiXErrorCodes errorCode, string errorDesc)
    {
        _localizationService = localizationService;
        ErrorCode = errorCode;
        ErrorDesc = errorDesc;
    }

    private readonly ILocalizationService _localizationService;

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
        return _localizationService.GetString("ErrorDescTemplate")
            .Replace("{0}", $"{ErrorCode}")
            .Replace("{1}", ErrorDesc);
    }
}