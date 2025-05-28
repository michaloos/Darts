using System.Collections.ObjectModel;
using System.Globalization;
using darts.Data.Model;

namespace darts.ValueConverters;

public class ShootRoundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ObservableCollection<UserGameShoot> shoots || parameter is not string param)
            return string.Empty;

        if (!int.TryParse(param, out int shotIndex))
            return string.Empty;

        // Liczymy, która to jest runda
        int round = shoots.Count / 3 + 1;  // Dla 3 strzałów na rundę

        // Pobieramy strzały na podstawie indeksu i rundy
        int startIndex = (round - 1) * 3;  // Początkowy indeks strzałów w tej rundzie
        int targetIndex = startIndex + shotIndex;

        // Jeśli mamy odpowiedni strzał, zwrócimy jego wynik, w przeciwnym razie pusty string
        return targetIndex < shoots.Count ? shoots[targetIndex].Score.ToString() : string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}