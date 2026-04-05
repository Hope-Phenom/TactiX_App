namespace TactiX_Models.Network;

/// <summary>
///     OAuth 登录 URL 响应
/// </summary>
[Serializable]
public class NLoginUrlResp
{
    /// <summary>
    ///     OAuth 提供商名称 (qq/wechat/dev)
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    ///     授权 URL
    /// </summary>
    public string LoginUrl { get; set; } = string.Empty;

    /// <summary>
    ///     状态令牌（GUID格式，用于防CSRF）
    /// </summary>
    public string State { get; set; } = string.Empty;
}