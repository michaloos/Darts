using CommunityToolkit.Maui.Views;
using darts.Core.Interface;
using darts.Core.Model;
using darts.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace darts.Services;

public class PopupService : IPopupService
{
    private readonly IServiceProvider _serviceProvider;

    public PopupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Wyświetla popup z określonym ViewModel i zwraca wynik
    /// </summary>
    public async Task<object?> ShowPopupAsync<TViewModel>(object? parameter = null) where TViewModel : class
    {
        // Utwórz instancję ViewModel przez kontener DI
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

        // Utwórz odpowiedni popup na podstawie typu ViewModel
        if (viewModel is NewGamePropsPopupViewModel propsViewModel && parameter is GameMode gameMode)
        {
            var popup = new NewGamePropsPopup(propsViewModel, gameMode);
            return await Application.Current.MainPage.ShowPopupAsync(popup);
        }

        // Dodaj obsługę innych typów popupów w razie potrzeby

        // Domyślny fallback
        throw new NotImplementedException($"Nie zaimplementowano obsługi popupu dla {typeof(TViewModel).Name}");
    }
}
