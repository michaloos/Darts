namespace darts.Services.Interfaces;

public interface ILoadingService
{
    Task<IDisposable> Show();
}