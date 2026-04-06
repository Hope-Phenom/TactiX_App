namespace TactiX_Models.Network;

/// <summary>
///     贡献者排行项
/// </summary>
[Serializable]
public class NTopUploader
{
    /// <summary>
    ///     排名
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    ///     用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///     昵称
    /// </summary>
    public string? Nickname { get; set; }

    /// <summary>
    ///     头像URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    ///     等级代码
    /// </summary>
    public string? LevelCode { get; set; }

    /// <summary>
    ///     等级名称
    /// </summary>
    public string? LevelName { get; set; }

    /// <summary>
    ///     上传数量
    /// </summary>
    public int UploadCount { get; set; }

    /// <summary>
    ///     总下载次数
    /// </summary>
    public uint TotalDownloadCount { get; set; }

    /// <summary>
    ///     总点赞次数
    /// </summary>
    public uint TotalLikeCount { get; set; }

    /// <summary>
    ///     综合质量评分
    /// </summary>
    public double QualityScore { get; set; }
}