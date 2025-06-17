using darts.Core.Interface;
using Mopups.Interfaces;
using Mopups.Services;

namespace darts.Services;

public class LoadingService : ILoadingService
{
    private readonly IPopupNavigation _navigation = MopupService.Instance;
    private LoadingPopup? _currentPopup;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<IDisposable> Show(string message)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_currentPopup != null)
            {
                _currentPopup.UpdateMessage(message);
            }
            else
            {
                _currentPopup = new LoadingPopup(message);
                await _navigation.PushAsync(_currentPopup);
            }
        }
        finally
        {
            _semaphore.Release();
        }

        return new LoadingInstance(this);
    }

    private class LoadingInstance(LoadingService service) : IDisposable
    {
        private bool _disposed;

        public async void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            await service._semaphore.WaitAsync();
            try
            {
                if (service._currentPopup != null)
                {
                    await service._navigation.RemovePageAsync(service._currentPopup);
                    service._currentPopup = null;
                }
            }
            finally
            {
                service._semaphore.Release();
            }
        }
    }
}