using darts.Core.Interface;
using darts.Core.Model;

namespace darts.Data.Context;

public class UnitOfWork(DartsDbContext dbContext) : IUnitOfWork
{
    public void Dispose() => dbContext.Dispose();

    public IRepository<User> Users { get; } = new Repository<User>(dbContext, dbContext.Users);
    public async Task<int> SaveAsync() => await dbContext.SaveChangesAsync();
}