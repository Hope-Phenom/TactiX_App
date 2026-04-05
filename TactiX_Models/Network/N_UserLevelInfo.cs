namespace TactiX_Models.Network;

/// <summary>
///     用户等级信息响应
/// </summary>
[Serializable]
public class NUserLevelInfo
{
    /// <summary>
    ///     等级代码 (normal/verified/pro/admin)
    /// </summary>
    public string LevelCode { get; set; } = "normal";

    /// <summary>
    ///     等级名称（如"普通用户")
    /// </summary>
    public string LevelName { get; set; } = string.Empty;

    /// <summary>
    ///     等级描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     徽章颜色（如"#95a5a6")
    /// </summary>
    public string? BadgeColor { get; set; }

    /// <summary>
    ///     单文件最大大小（字节）
    /// </summary>
    public uint MaxFileSize { get; set; }

    /// <summary>
    ///     允许上传的文件总数
    /// </summary>
    public uint MaxUploadCount { get; set; }

    /// <summary>
    ///     每日上传限制
    /// </summary>
    public uint DailyUploadLimit { get; set; }

    /// <summary>
    ///     是否即时通知审核结果
    /// </summary>
    public bool InstantNotification { get; set; }
}