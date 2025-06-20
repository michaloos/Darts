using System.Globalization;

namespace darts.ValueConverters;

public class IsSelectedItemConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return false;
        
        var collectionView = (value as BindableObject)?.BindingContext as CollectionView;
        var item = (value as BindableObject)?.BindingContext;
        
        return collectionView?.SelectedItem == item;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}