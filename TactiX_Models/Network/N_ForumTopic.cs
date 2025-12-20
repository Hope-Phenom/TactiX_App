namespace TactiX_Models.Network;

/// <summary>
///     论坛热帖
/// </summary>
[Serializable]
public class NForumTopic
{
    /// <summary>
    ///     标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    ///     url地址
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    ///     更新时间
    /// </summary>
    public required string Date { get; set; }
}