using darts.Core.Interface;
using darts.Core.Model;

namespace darts.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<IReadOnlyList<User>> GetAllAsync() => await unitOfWork.Users.GetAllAsync();

    public async Task<User?> GetByIdAsync(Guid id) => await unitOfWork.Users.GetByIdAsync(id);

    public async Task AddAsync(User user) => await unitOfWork.Users.AddAsync(user);

    public void UpdateAsync(User user) => unitOfWork.Users.Update(user);

    public void DeleteAsync(User user) => unitOfWork.Users.Delete(user);
}