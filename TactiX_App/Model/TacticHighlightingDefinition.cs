using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia.Media;
using AvaloniaEdit.Highlighting;

namespace TactiX_App.Model;

public class TacticHighlightingDefinition : IHighlightingDefinition
{
    public const string CommentRuleName = "Comment";
    public const string KeywordRuleName = "Keyword";

    private readonly List<string> _keyWords =
    [
        "Name", "Author", "Description", "ApplicableVersion", "TacticType",
        "TacVersion", "UpdateTime", "ModName", "ModVersion", "Actions"
    ];

    public TacticHighlightingDefinition()
    {
        MainRuleSet = new HighlightingRuleSet();

        AddCommentRule();
        AddKeywordRule(_keyWords);
    }

    public string Name => "TactiX";
    public HighlightingRuleSet MainRuleSet { get; }

    public IEnumerable<HighlightingColor> NamedHighlightingColors
    {
        get
        {
            yield return GetNamedColor(CommentRuleName);
            yield return GetNamedColor(KeywordRuleName);
        }
    }

    public IDictionary<string, string> Properties => new Dictionary<string, string>();

    public HighlightingColor GetNamedColor(string name)
    {
        // 根据名称返回颜色
        return name switch
        {
            CommentRuleName => new HighlightingColor { Foreground = new SimpleHighlightingBrush(Colors.DarkGreen) },
            KeywordRuleName => new HighlightingColor
                { Foreground = new SimpleHighlightingBrush(Colors.DeepSkyBlue), FontWeight = FontWeight.Bold },
            _ => new HighlightingColor { Foreground = new SimpleHighlightingBrush(Colors.DarkGreen) }
        };
    }

    public HighlightingRuleSet GetNamedRuleSet(string name)
    {
        return MainRuleSet;
    }

    private void AddCommentRule()
    {
        // 创建注释规则
        var commentRule = new HighlightingRule
        {
            Regex = new Regex(@"//.*"),
            Color = new HighlightingColor
            {
                Foreground = new SimpleHighlightingBrush(Colors.DarkGreen)
            }
        };

        MainRuleSet.Rules.Add(commentRule);
    }

    private void AddKeywordRule(IEnumerable<string> keywords)
    {
        // 创建关键词正则表达式
        var keywordPattern = $@"\b({string.Join("|", keywords)})\b";

        // 创建关键词规则
        var keywordRule = new HighlightingRule
        {
            Regex = new Regex(keywordPattern),
            Color = new HighlightingColor
            {
                Foreground = new SimpleHighlightingBrush(Colors.DeepSkyBlue),
                FontWeight = FontWeight.Bold
            }
        };

        MainRuleSet.Rules.Add(keywordRule);
    }
}