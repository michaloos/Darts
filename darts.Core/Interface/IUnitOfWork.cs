using darts.Core.Model;

namespace darts.Core.Interface;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    Task<int> SaveAsync();
}