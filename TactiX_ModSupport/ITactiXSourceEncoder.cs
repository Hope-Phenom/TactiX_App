using TactiX_Models.Tactics;

namespace TactiX_ModSupport;

public interface ITactiXSourceEncoder
{
    /// <summary>
    ///     解析传入的文本为战术文件
    /// </summary>
    public LTactic? Decoder(string[] lines);
}