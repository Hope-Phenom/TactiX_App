namespace TactiX_Models.Network;

/// <summary>
///     战术文件版本列表响应
/// </summary>
[Serializable]
public class NTacticsVersionsResult
{
    /// <summary>
    ///     配装码
    /// </summary>
    public string ShareCode { get; set; } = string.Empty;

    /// <summary>
    ///     版本列表
    /// </summary>
    public List<NTacticsVersion> Versions { get; set; } = new();
}