namespace TactiX_Models.Network;

/// <summary>
///     系统公告
/// </summary>
[Serializable]
public class NNewsSys
{
    /// <summary>
    ///     标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     链接
    /// </summary>
    public string Link { get; set; } = string.Empty;

    /// <summary>
    ///     更新日期
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    ///     日期二次封装
    /// </summary>
    public string DateTimeStr => DateTime.Now.ToString("yyyy-MM-dd");
    
    /// <summary>
    ///     是否为最后一条，用于控制分割线显示
    /// </summary>
    public bool IsLast { get; set; }
}