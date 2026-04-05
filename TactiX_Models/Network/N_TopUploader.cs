namespace TactiX_Models.Network;

/// <summary>
///     贡献者排行项
/// </summary>
[Serializable]
public class NTopUploader
{
    /// <summary>
    ///     用户信息
    /// </summary>
    public NUserBrief User { get; set; } = new();

    /// <summary>
    ///     上传文件数量
    /// </summary>
    public int UploadCount { get; set; }

    /// <summary>
    ///     总下载次数
    /// </summary>
    public uint TotalDownloads { get; set; }

    /// <summary>
    ///     总点赞数
    /// </summary>
    public uint TotalLikes { get; set; }

    /// <summary>
    ///     排名
    /// </summary>
    public int Rank { get; set; }
}