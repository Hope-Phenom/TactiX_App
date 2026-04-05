namespace TactiX_Models.Network;

/// <summary>
///     热门战术排行项
/// </summary>
[Serializable]
public class NHotFile
{
    /// <summary>
    ///     战术详情
    /// </summary>
    public NTacticsDetail? Detail { get; set; }

    /// <summary>
    ///     排名
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    ///     热度分数 (qualityScore = downloadCount × 0.3 + likeCount × 0.7)
    /// </summary>
    public uint HeatScore { get; set; }
}