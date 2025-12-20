namespace TactiX_Models.Tactics;

/// <summary>
///     战术步骤，使用ID和MOD项进行关联
/// </summary>
[Serializable]
public class L_TacticAction
{
    /// <summary>
    ///     动作对象，只记录基础信息和MOD中的映射关系
    /// </summary>
    public L_TacticAction()
    {
    }

    /// <summary>
    ///     动作对象，只记录基础信息和MOD中的映射关系
    /// </summary>
    /// <param name="stepNo"></param>
    /// <param name="itemAbbr"></param>
    /// <param name="time"></param>
    /// <param name="number"></param>
    public L_TacticAction(ushort stepNo, string itemAbbr, uint time, int number = 1)
    {
        Step = stepNo;
        ItemAbbr = itemAbbr;
        Time = time;
        Number = number;
    }

    /// <summary>
    ///     步骤步数
    /// </summary>
    public ushort Step { get; set; }

    /// <summary>
    ///     步骤对应MOD单位的缩写
    /// </summary>
    public string ItemAbbr { get; set; } = string.Empty;

    /// <summary>
    ///     时间/秒
    /// </summary>
    public uint Time { get; set; }

    /// <summary>
    ///     人口情况
    /// </summary>
    public string Supply { get; set; } = string.Empty;

    /// <summary>
    ///     （单位）个数，默认为1
    /// </summary>
    public int Number { get; set; } = 1;

    public override string ToString()
    {
        var sec = Time % 60;
        var min = (Time - sec) / 60;
        var timeStr = min.ToString().PadLeft(2, '0') + ":" + sec.ToString().PadLeft(2, '0');

        return $"{timeStr}, {ItemAbbr}*{Number}, {Supply}";
    }
}