namespace TactiX_Models.MessageBus;

public class MbWindowClose
{
    /// <summary>
    ///     用于进行消息配对，避免错误关闭窗体
    /// </summary>
    public required string Name { get; set; }
}