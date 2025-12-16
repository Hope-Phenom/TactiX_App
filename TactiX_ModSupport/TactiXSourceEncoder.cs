using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TactiX_Models.Tactics;

namespace TactiX_ModSupport
{
    /// <summary>
    /// 临时用，单纯的字符串操作；
    /// 后续应该优化此编译器甚至战术文件的格式
    /// </summary>
    public class TactiXSourceEncoder : ITactiXSourceEncoder
    {
        public TactiXSourceEncoder() { }

        public L_Tactic? Decoder(string[] lines)
        {
            try
            {
                if (lines.Length == 0) return null;

                var tactix = new L_Tactic();

                foreach (var line in lines)
                {
                    string _line = line
                        .Replace(" ","")
                        .Replace("：", ":")
                        .Replace("，", ",");

                    // Actions 需要特殊处理
                    var condtion_action = _line.StartsWith('-');
                    // 空行或者注释直接跳过
                    var condtion_useless = _line.Equals(Environment.NewLine)
                        || _line.StartsWith("//")
                        || _line.StartsWith("Actions:")
                        || string.IsNullOrEmpty(_line);
                    // 这几个字段是uint值
                    var condition_uint = _line.StartsWith("TacVersion")
                        ||  _line.StartsWith("ModVersion");
                    // 战术文件的类别
                    var condtion_tacticType = _line.StartsWith("TacticType");

                    if (condtion_action)
                    {
                        var strs = _line.Replace("-", "").Split(',');
                        if (strs.Length != 3 && strs.Length != 4) continue;

                        var action = new L_TacticAction();
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
                            action.Supply = strs.Length == 4
                                ? strs[3]
                                : string.Empty;
                        }

                        tactix.Actions.Add(action);
                    }
                    else if (condtion_useless)
                    {
                        continue;
                    }
                    else
                    {
                        var propertyName = _line[.._line.IndexOf(':')];
                        var property = tactix.GetType().GetProperty(propertyName);
                        if (property == null) continue;

                        var value = _line[(_line.IndexOf(':') + 1)..];

                        if (condition_uint)
                        {
                            property.SetValue(tactix, Convert.ToUInt32(value));
                        }
                        else if (condtion_tacticType)
                        {
                            var typeEnum = value == "Timeline"
                                ? L_TacticEnum.TIMELINE
                                : L_TacticEnum.STEP;
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
            if (TimeSpan.TryParseExact(timeString, "m\\:ss", null, out TimeSpan timeSpan))
            {
                return (uint)timeSpan.TotalSeconds;
            }

            throw new FormatException("时间格式应为 分:秒");
        }
    }
}
