using System.Collections.ObjectModel;
using System.Globalization;
using darts.Core.Model;

namespace darts.ValueConverters;

public class ShootRoundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ObservableCollection<UserGameShoot> shoots || parameter is not string param)
            return string.Empty;

        if (!int.TryParse(param, out int shotIndex))
            return string.Empty;

        int round = shoots.Count / 3 + 1; 
        
        int startIndex = (round - 1) * 3; 
        int targetIndex = startIndex + shotIndex;
        
        return targetIndex < shoots.Count ? shoots[targetIndex].Score.ToString() : string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}