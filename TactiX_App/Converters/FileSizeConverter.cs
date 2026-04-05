using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TactiX_App.Converters;

/// <summary>
///     文件大小格式化转换器
/// </summary>
public class FileSizeConverter : IValueConverter
{
    public static readonly FileSizeConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is long size)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int i = 0;
            double displaySize = size;

            while (displaySize >= 1024 && i < suffixes.Length - 1)
            {
                displaySize /= 1024;
                i++;
            }

            return $"{displaySize:F2} {suffixes[i]}";
        }

        return "0 B";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}