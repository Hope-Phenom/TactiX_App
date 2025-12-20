namespace TactiX_Models.MessageBus;

public class MbFolderDialog
{
    /// <summary>
    ///     请求的窗体名
    /// </summary>
    public required string WindowName { get; set; }

    /// <summary>
    ///     用于区分作用
    /// </summary>
    public required string Trigger { get; set; }

    /// <summary>
    ///     目录地址
    /// </summary>
    public string? FolderPath { get; set; }

    /// <summary>
    ///     建议初始目录
    /// </summary>
    public string? SuggestStartLocation { get; set; }
}