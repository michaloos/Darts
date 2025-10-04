using CommunityToolkit.Maui.Extensions;
using darts.Core.Interface;
using darts.Core.Model;
using darts.ViewModel;

namespace darts.Services;

public class PopupService(IServiceProvider serviceProvider) : IPopupService
{
    public async Task<object?> ShowPopupAsync<TViewModel>(object? parameter = null) where TViewModel : class
    {
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();

        if (viewModel is NewGamePropsPopupViewModel propsViewModel && parameter is GameMode gameMode)
        {
            var popup = new NewGamePropsPopup(propsViewModel, gameMode);
            _ = App.Current.Windows[0].Page.ShowPopupAsync(popup);
            return await popup.Result;
        }

        throw new NotImplementedException($"Nie zaimplementowano obsługi popupu dla {typeof(TViewModel).Name}");
    }
}
