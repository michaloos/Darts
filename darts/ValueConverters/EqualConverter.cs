using System.Globalization;

namespace darts.ValueConverters;

public class EqualConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Sprawdza, czy value jest równe parameter
        if (value == null || parameter == null)
            return false;

        // Obsługa konwersji typów, np. string -> int dla punktów startowych
        if (value is int intValue && int.TryParse(parameter.ToString(), out int intParam))
            return intValue == intParam;

        return value.Equals(parameter);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Jeśli RadioButton jest zaznaczony, zwróć wartość parametru
        if (value is bool and true && parameter != null)
        {
            // Jeśli targetType to int, konwertuj parametr na int
            if (targetType == typeof(int) && int.TryParse(parameter.ToString(), out int intParam))
                return intParam;

            return parameter;
        }

        // W przeciwnym przypadku zwróć null (lub wartość domyślną dla typu)
        return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
    }
}
