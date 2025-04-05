using darts.Services.Interfaces;
using Mopups.Interfaces;
using Mopups.Services;

namespace darts.Services;

public class LoadingService : ILoadingService, IDisposable
{
    private readonly IPopupNavigation navigation = MopupService.Instance;

    public async Task<IDisposable> Show()
    {
        await navigation.PushAsync(new LoadingPopup("Tworzenie gry"), true);
        return this;
    }

    public async void Dispose()
    {
        await navigation.PopAsync();
    }
}