using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TactiX_App.Converters;

/// <summary>
///     种族代码转颜色转换器 (P=金色, T=蓝色, Z=紫色)
/// </summary>
public class RaceToColorConverter : IValueConverter
{
    public static readonly RaceToColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var race = value as string;
        return race?.ToUpperInvariant() switch
        {
            "P" => new SolidColorBrush(Color.Parse("#FFD700")), // Protoss - 金色
            "T" => new SolidColorBrush(Color.Parse("#4A90D9")), // Terran - 蓝色
            "Z" => new SolidColorBrush(Color.Parse("#9B59B6")), // Zerg - 紫色
            _ => new SolidColorBrush(Color.Parse("#4A90D9"))    // 默认蓝色
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}