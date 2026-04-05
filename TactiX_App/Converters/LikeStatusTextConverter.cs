using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TactiX_App.Converters;

/// <summary>
///     点赞状态转按钮文本转换器
/// </summary>
public class LikeStatusTextConverter : IValueConverter
{
    public static readonly LikeStatusTextConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isLiked && isLiked)
        {
            return "取消点赞";
        }

        return "点赞";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}