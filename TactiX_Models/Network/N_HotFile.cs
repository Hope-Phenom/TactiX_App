namespace TactiX_Models.Network;

/// <summary>
///     热门战术排行项
/// </summary>
[Serializable]
public class NHotFile
{
    /// <summary>
    ///     排名
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    ///     配装码
    /// </summary>
    public string ShareCode { get; set; } = string.Empty;

    /// <summary>
    ///     战术名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     作者
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    ///     种族代码
    /// </summary>
    public string? Race { get; set; }

    /// <summary>
    ///     种族显示名称
    /// </summary>
    public string? RaceDisplay { get; set; }

    /// <summary>
    ///     下载次数
    /// </summary>
    public uint DownloadCount { get; set; }

    /// <summary>
    ///     点赞次数
    /// </summary>
    public uint LikeCount { get; set; }

    /// <summary>
    ///     收藏次数
    /// </summary>
    public uint FavoriteCount { get; set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     上传者信息
    /// </summary>
    public NUserBrief? Uploader { get; set; }
}