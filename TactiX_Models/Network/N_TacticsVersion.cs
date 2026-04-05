namespace TactiX_Models.Network;

/// <summary>
///     战术文件版本信息
/// </summary>
[Serializable]
public class NTacticsVersion
{
    /// <summary>
    ///     版本号
    /// </summary>
    public int VersionNumber { get; set; }

    /// <summary>
    ///     上传时间
    /// </summary>
    public DateTime UploadedAt { get; set; }

    /// <summary>
    ///     文件大小（字节）
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    ///     版本描述/变更日志
    /// </summary>
    public string? Changelog { get; set; }

    /// <summary>
    ///     上传者信息
    /// </summary>
    public NUserBrief? Uploader { get; set; }
}