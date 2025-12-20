namespace TactiX_Models.MessageBus;

public class MbToastPureText
{
    /// <summary>
    ///     提示类型
    /// </summary>
    public MbEnumToastType Type { get; set; }

    /// <summary>
    ///     提示标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///     提示文本
    /// </summary>
    public required string Message { get; set; }
}