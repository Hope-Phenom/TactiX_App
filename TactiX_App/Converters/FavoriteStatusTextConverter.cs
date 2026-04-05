using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TactiX_App.Converters;

/// <summary>
///     收藏状态转按钮文本转换器
/// </summary>
public class FavoriteStatusTextConverter : IValueConverter
{
    public static readonly FavoriteStatusTextConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isFavorited && isFavorited)
        {
            return "取消收藏";
        }

        return "收藏";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}