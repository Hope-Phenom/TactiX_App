namespace TactiX_Models.Network;

/// <summary>
///     点赞操作结果
/// </summary>
[Serializable]
public class NLikeResult
{
    /// <summary>
    ///     操作后是否已点赞
    /// </summary>
    public bool IsLiked { get; set; }

    /// <summary>
    ///     当前点赞总数
    /// </summary>
    public uint LikeCount { get; set; }

    /// <summary>
    ///     结果消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
}