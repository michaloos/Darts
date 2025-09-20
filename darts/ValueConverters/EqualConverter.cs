using System.Globalization;

namespace darts.ValueConverters;

public class EqualConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is null || value is null)
            return false;
        
        object param = parameter;
        if (value is int && parameter is string ps && int.TryParse(ps, out var pi))
            param = pi;

        return Equals(value, param);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b)
        {
            if (parameter is string ps && int.TryParse(ps, out var pi))
                return pi;

            return parameter ?? Binding.DoNothing;
        }

        return Binding.DoNothing;
    }
}