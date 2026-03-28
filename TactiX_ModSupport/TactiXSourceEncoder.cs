using TactiX_Models.Tactics;

namespace TactiX_ModSupport;

/// <summary>
///     临时用，单纯的字符串操作；
///     后续应该优化此编译器甚至战术文件的格式
/// </summary>
public class TactiXSourceEncoder : ITactiXSourceEncoder
{
    public LTactic? Decoder(string[] lines)
    {
        try
        {
            if (lines.Length == 0) return null;

            var tactix = new LTactic();

            foreach (var line in lines)
            {
                var newLine = line
                    .Replace(" ", "")
                    .Replace("：", ":")
                    .Replace("，", ",");

                // Actions 需要特殊处理
                var conditionAction = newLine.StartsWith('-');
                // 空行或者注释直接跳过
                var conditionUseless = newLine.Equals(Environment.NewLine)
                                       || newLine.StartsWith("//")
                                       || newLine.StartsWith("Actions:")
                                       || string.IsNullOrEmpty(newLine);
                // 这几个字段是uint值
                var conditionUint = newLine.StartsWith("TacVersion")
                                     || newLine.StartsWith("ModVersion");
                // 战术文件的类别
                var conditionTacticType = newLine.StartsWith("TacticType");

                if (conditionAction)
                {
                    var strs = newLine.Replace("-", "").Split(',');
                    if (strs.Length != 3 && strs.Length != 4) continue;

                    var action = new LTacticAction();
                    action.Step = Convert.ToUInt16(strs[0]);
                    action.Time = TimeParser.ConvertToSeconds(strs[1]);

                    // 判断是否添加了个数标签
                    if (strs[2].IndexOf("*") != -1)
                    {
                        var arr = strs[2].Split('*');
                        var abbr = arr[0];
                        var number = Convert.ToInt32(arr[1]);

                        action.ItemAbbr = abbr;
                        action.Number = number;
                    }
                    else
                    {
                        action.ItemAbbr = strs[2];
                    }

                    // 统一处理 Supply（修复 Bug：无论是否有 *number 都应该处理）
                    if (strs.Length == 4)
                        action.Supply = strs[3];

                    tactix.Actions.Add(action);
                }
                else if (conditionUseless)
                {
                }
                else
                {
                    var propertyName = newLine[..newLine.IndexOf(':')];
                    var property = tactix.GetType().GetProperty(propertyName);
                    if (property == null) continue;

                    var value = newLine[(newLine.IndexOf(':') + 1)..];

                    if (conditionUint)
                    {
                        property.SetValue(tactix, Convert.ToUInt32(value));
                    }
                    else if (conditionTacticType)
                    {
                        var typeEnum = value == "Timeline"
                            ? LTacticEnum.Timeline
                            : LTacticEnum.Step;
                        property.SetValue(tactix, typeEnum);
                    }
                    // 剩下的都是字符串类型
                    else
                    {
                        property.SetValue(tactix, value);
                    }
                }
            }

            return tactix;
        }
        catch
        {
            return null;
        }
    }
}

internal static class TimeParser
{
    public static uint ConvertToSeconds(string timeString)
    {
        // 使用 TimeSpan 解析
        if (TimeSpan.TryParseExact(timeString, "m\\:ss", null, out var timeSpan)) return (uint)timeSpan.TotalSeconds;

        throw new FormatException("时间格式应为 分:秒");
    }
}