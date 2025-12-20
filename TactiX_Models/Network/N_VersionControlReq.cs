namespace TactiX_Models.Network;

[Serializable]
public class NVersionControlReq
{
    /// <summary>
    ///     版本号
    /// </summary>
    public required string Version { get; set; }
}