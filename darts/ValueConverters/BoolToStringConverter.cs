using System.Globalization;

namespace darts.ValueConverters;

public class BoolToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not string param)
            return value?.ToString() ?? string.Empty;

        var parts = param.Split(',');
        if (parts.Length != 2)
            return value?.ToString() ?? string.Empty;

        var trueValue = parts[0];
        var falseValue = parts[1];
        
        if (value is bool boolValue)
            return boolValue ? trueValue : falseValue;
        
        if (value is null)
            return falseValue;
        
        if (value is int intValue)
            return intValue != 0 ? trueValue : falseValue;

        if (value is string stringValue)
            return !string.IsNullOrWhiteSpace(stringValue) ? trueValue : falseValue;
        
        return value != null ? trueValue : falseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not string param)
            return false;

        var parts = param.Split(',');
        if (parts.Length != 2)
            return false;

        var trueValue = parts[0];
        
        return value?.ToString() == trueValue;
    }
}