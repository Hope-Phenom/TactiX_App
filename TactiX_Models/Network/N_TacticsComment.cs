namespace TactiX_Models.Network;

/// <summary>
///     战术评论模型
/// </summary>
[Serializable]
public class NTacticsComment
{
    /// <summary>
    ///     评论ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     配装码
    /// </summary>
    public required string ShareCode { get; set; }

    /// <summary>
    ///     评论内容
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     父评论ID（用于嵌套回复）
    /// </summary>
    public long? ParentCommentId { get; set; }

    /// <summary>
    ///     评论作者
    /// </summary>
    public NUserBrief? Author { get; set; }

    /// <summary>
    ///     是否已被删除
    /// </summary>
    public bool IsDeleted { get; set; }
}

/// <summary>
///     评论添加请求
/// </summary>
[Serializable]
public class NAddCommentReq
{
    /// <summary>
    ///     评论内容
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    ///     父评论ID（回复评论时使用）
    /// </summary>
    public long? ParentCommentId { get; set; }
}