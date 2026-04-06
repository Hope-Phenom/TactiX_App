namespace TactiX_Models.Network;

/// <summary>
///     评论列表响应
/// </summary>
[Serializable]
public class NTacticsCommentResult
{
    /// <summary>
    ///     总数量
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    ///     当前页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    ///     每页数量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    ///     评论列表
    /// </summary>
    public List<NTacticsComment> Comments { get; set; } = new();
}