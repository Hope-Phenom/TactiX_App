namespace TactiX_Models.Network;

/// <summary>
///     战术搜索结果响应模型
/// </summary>
[Serializable]
public class NTacticsSearchResult
{
    /// <summary>
    ///     战术列表
    /// </summary>
    public List<NTacticsDetail> Items { get; set; } = new();

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
    ///     总页数
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
///     战术搜索请求参数
/// </summary>
[Serializable]
public class NTacticsSearchReq
{
    /// <summary>
    ///     搜索关键词
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    ///     种族筛选 (P/T/Z 或 null 表示全部)
    /// </summary>
    public string? Race { get; set; }

    /// <summary>
    ///     上传者ID
    /// </summary>
    public long? UploaderId { get; set; }

    /// <summary>
    ///     排序方式 (latest/popular/downloads)
    /// </summary>
    public string SortBy { get; set; } = "latest";

    /// <summary>
    ///     页码
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    ///     每页数量
    /// </summary>
    public int PageSize { get; set; } = 20;
}