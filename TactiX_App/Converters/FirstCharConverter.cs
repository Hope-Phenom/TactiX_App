using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TactiX_App.Converters;

/// <summary>
///     字符串首字符转换器 (用于显示用户头像首字母)
/// </summary>
public class FirstCharConverter : IValueConverter
{
    public static readonly FirstCharConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str && !string.IsNullOrEmpty(str))
        {
            return str[0].ToString().ToUpperInvariant();
        }

        return "?";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}