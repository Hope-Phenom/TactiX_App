namespace TactiX_Models.Network;

/// <summary>
///     用户会话信息（OAuth 登录响应）
/// </summary>
[Serializable]
public class NUserSession
{
    /// <summary>
    ///     结果消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     用户唯一ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///     用户昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    ///     用户头像URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    ///     用户等级代码 (normal/verified/pro/admin)
    /// </summary>
    public string LevelCode { get; set; } = "normal";

    /// <summary>
    ///     JWT 访问令牌
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    ///     刷新令牌
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    ///     Token 有效期（秒）
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    ///     用户是否已登录
    /// </summary>
    public bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);

    /// <summary>
    ///     Token 过期时间（计算属性）
    /// </summary>
    public DateTime ExpiresAt => DateTime.UtcNow.AddSeconds(ExpiresIn);
}