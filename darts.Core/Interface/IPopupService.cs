namespace darts.Core.Interface;

/// <summary>
/// Interfejs serwisu do wyświetlania popupów w aplikacji
/// </summary>
public interface IPopupService
{
    /// <summary>
    /// Wyświetla popup z określonym ViewModel i zwraca wynik
    /// </summary>
    /// <typeparam name="TViewModel">Typ ViewModel używany przez popup</typeparam>
    /// <param name="parameter">Opcjonalny parametr do inicjalizacji ViewModel</param>
    /// <returns>Obiekt zwrócony przez popup lub null</returns>
    Task<object?> ShowPopupAsync<TViewModel>(object? parameter = null) where TViewModel : class;
}
