namespace TactiX_Models.MessageBus;

public class MbToastVersion : MbToastPureText
{
    /// <summary>
    ///     版本发布地址，用于网页跳转
    /// </summary>
    public required string ReleaseUrl { get; set; }
}