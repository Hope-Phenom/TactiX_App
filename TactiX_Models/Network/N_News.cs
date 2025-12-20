namespace TactiX_Models.Network;

/// <summary>
///     新闻接口返回对象
/// </summary>
[Serializable]
public class NNews
{
    public int Id { get; set; }

    /// <summary>
    ///     更新日期
    /// </summary>
    public DateTime UpdateDateTime { get; set; }

    /// <summary>
    ///     Json文本
    /// </summary>
    public string Json { get; set; } = string.Empty;

    /// <summary>
    ///     类别，0-社区热帖，1-Bilibili视频推荐
    /// </summary>
    public int Type { get; set; }
}