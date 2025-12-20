namespace TactiX_Models.Tactics;

/// <summary>
///     Mod文件的描述类，包括基本信息和动作、单位映射关系
/// </summary>
[Serializable]
public class LModDesc
{
    /// <summary>
    ///     Mod名称
    /// </summary>
    public required string ModName { get; set; }

    /// <summary>
    ///     Mod文件路径
    /// </summary>
    public required string TacticsPath { get; set; }

    /// <summary>
    ///     Mod版本号
    /// </summary>
    public ulong ModVersion { get; set; } = ulong.MinValue;

    /// <summary>
    ///     作者，展示用
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    ///     作者的联系邮箱
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    ///     MOD项目的主页，可以是论坛发布帖的地址或个人网站等等，也可以是QQ频道的加入链接
    /// </summary>
    public required string ProjectWebSite { get; set; }

    /// <summary>
    ///     更新时间
    /// </summary>
    public required string UpdateTime { get; set; }

    /// <summary>
    ///     Mod自描述
    /// </summary>
    public required string Desc { get; set; }

    /// <summary>
    ///     动作的映射关系
    /// </summary>
    public List<LModItem> Actions { get; set; } = new();

    /// <summary>
    ///     单位的映射关系
    /// </summary>
    public List<LModItem> Units { get; set; } = new();
}