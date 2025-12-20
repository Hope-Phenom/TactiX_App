namespace TactiX_Models.MessageBus;

public class MB_WindowClose
{
    /// <summary>
    ///     用于进行消息配对，避免错误关闭窗体
    /// </summary>
    public required string Name { get; set; }
}