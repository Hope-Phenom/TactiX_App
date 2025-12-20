namespace TactiX_Models.Network;

/// <summary>
///     异常上报数据结构
/// </summary>
[Serializable]
public class NExceptionReportModel
{
    /// <summary>
    ///     错误代码
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    ///     错误描述
    /// </summary>
    public string ErrorDesc { get; set; } = string.Empty;

    /// <summary>
    ///     用户反馈渠道
    /// </summary>
    public string FeedbackWay { get; set; } = string.Empty;

    /// <summary>
    ///     用户反馈信息
    /// </summary>
    public string FeedbackInfo { get; set; } = string.Empty;

    /// <summary>
    ///     反馈创建日期
    /// </summary>
    public DateTime CreateTime { get; set; }
}