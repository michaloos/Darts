using darts.Core.Model;

namespace darts.Core.Interface;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    void UpdateAsync(User user);
    void DeleteAsync(User user);
}