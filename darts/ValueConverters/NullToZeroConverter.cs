using System.Globalization;
using Microsoft.Maui.Controls;

namespace darts.ValueConverters;

public class NullToZeroConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return 0;

        if (value is int i) return i;

        if (value is int ni) return ni;

        if (value is long l) return l > int.MaxValue ? int.MaxValue : (int)l;

        if (value is long nl) return nl > int.MaxValue ? int.MaxValue : (int)nl;

        if (value is double d) return (int)d;

        if (value is double nd) return (int)nd;

        if (value is float f) return (int)f;

        if (value is float nf) return (int)nf;

        if (value is decimal de) return (int)de;

        if (value is decimal nde) return (int)nde;

        if (value is string s)
            return int.TryParse(s, NumberStyles.Any, culture, out var parsed) ? parsed : 0;

        try
        {
            return System.Convert.ToInt32(value, culture);
        }
        catch
        {
            return 0;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value ?? 0;
    }
}