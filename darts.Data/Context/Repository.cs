using darts.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace darts.Data.Context;

public class Repository<T>(DartsDbContext context, DbSet<T> dbSet) : IRepository<T>
    where T : class, IEntity
{
    public async Task<IReadOnlyList<T>> GetAllAsync() => await dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(object id) => await dbSet.FindAsync(id);

    public async Task AddAsync(T entity) => await dbSet.AddAsync(entity);

    public void Update(T entity) => dbSet.Update(entity);

    public void Delete(T entity) => dbSet.Remove(entity);

    public async Task SaveAsync() => await context.SaveChangesAsync();
}