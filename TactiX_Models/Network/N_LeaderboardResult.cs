namespace TactiX_Models.Network;

/// <summary>
///     热门战术排行榜响应
/// </summary>
[Serializable]
public class NHotFilesResult
{
    /// <summary>
    ///     时间周期
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    ///     种族筛选
    /// </summary>
    public string? Race { get; set; }

    /// <summary>
    ///     排序方式
    /// </summary>
    public string SortBy { get; set; } = string.Empty;

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
    ///     战术列表
    /// </summary>
    public List<NHotFile> Files { get; set; } = new();
}

/// <summary>
///     贡献者排行榜响应
/// </summary>
[Serializable]
public class NTopUploadersResult
{
    /// <summary>
    ///     时间周期
    /// </summary>
    public string Period { get; set; } = string.Empty;

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
    ///     贡献者列表
    /// </summary>
    public List<NTopUploader> Uploaders { get; set; } = new();
}