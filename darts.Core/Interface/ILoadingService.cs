namespace darts.Core.Interface;

public interface ILoadingService
{
    Task<IDisposable> Show();
}