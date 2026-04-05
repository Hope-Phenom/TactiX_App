using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TactiX_App.Converters;

/// <summary>
///     Bool 转颜色转换器 (用于点赞/收藏状态显示)
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    public static readonly BoolToColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isActive && isActive)
        {
            return new SolidColorBrush(Color.Parse("#FF6B6B")); // 活跃状态 - 红色
        }

        return new SolidColorBrush(Color.Parse("#FFFFFF")); // 非活跃状态 - 白色
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}