namespace TactiX_Models.Network;

/// <summary>
///     收藏操作结果
/// </summary>
[Serializable]
public class NFavoriteResult
{
    /// <summary>
    ///     操作后是否已收藏
    /// </summary>
    public bool IsFavorited { get; set; }

    /// <summary>
    ///     当前收藏总数
    /// </summary>
    public uint FavoriteCount { get; set; }

    /// <summary>
    ///     结果消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
}