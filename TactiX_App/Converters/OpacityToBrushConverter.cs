using System;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TactiX_App.Converters;

public class OpacityToBrushConverter : IValueConverter
{
    public static readonly OpacityToBrushConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is double opacity)
        {
            // 创建一个带有指定透明度的黑色SolidColorBrush
            return new SolidColorBrush(Colors.Black, opacity);
        }
        
        // 默认返回完全不透明的黑色
        return new SolidColorBrush(Colors.Black, 1.0);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
