namespace TactiX_Models.Network;

/// <summary>
///     战术文件详情响应模型
/// </summary>
[Serializable]
public class NTacticsDetail
{
    /// <summary>
    ///     配装码（8字符62进制）
    /// </summary>
    public required string ShareCode { get; set; }

    /// <summary>
    ///     战术名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     作者
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    ///     种族代码 (P/T/Z)
    /// </summary>
    public string? Race { get; set; }

    /// <summary>
    ///     种族显示名称（如"神族")
    /// </summary>
    public string? RaceDisplay { get; set; }

    /// <summary>
    ///     文件大小（字节）
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    ///     下载数量
    /// </summary>
    public uint DownloadCount { get; set; }

    /// <summary>
    ///     点赞数量
    /// </summary>
    public uint LikeCount { get; set; }

    /// <summary>
    ///     收藏数量
    /// </summary>
    public uint FavoriteCount { get; set; }

    /// <summary>
    ///     当前用户是否已点赞
    /// </summary>
    public bool IsLikedByUser { get; set; }

    /// <summary>
    ///     当前用户是否已收藏
    /// </summary>
    public bool IsFavoritedByUser { get; set; }

    /// <summary>
    ///     当前版本号
    /// </summary>
    public int CurrentVersion { get; set; }

    /// <summary>
    ///     文件状态 (pending/approved/rejected)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    ///     上传者信息
    /// </summary>
    public NUserBrief? Uploader { get; set; }
}

/// <summary>
///     用户简要信息
/// </summary>
[Serializable]
public class NUserBrief
{
    /// <summary>
    ///     用户唯一ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     用户昵称
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    ///     头像URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    ///     用户等级代码 (normal/verified/pro/admin)
    /// </summary>
    public string? LevelCode { get; set; }

    /// <summary>
    ///     用户等级名称
    /// </summary>
    public string? LevelName { get; set; }
}