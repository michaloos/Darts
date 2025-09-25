using System.Globalization;

namespace darts.ValueConverters;

public class InitialsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string name || string.IsNullOrWhiteSpace(name))
            return "?";

        var words = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        if (words.Length == 0)
            return "?";
        
        if (words.Length == 1)
            return words[0].Substring(0, Math.Min(2, words[0].Length)).ToUpper();
        
        var first = words[0].Substring(0, 1).ToUpper();
        var last = words[words.Length - 1].Substring(0, 1).ToUpper();
        
        return first + last;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}