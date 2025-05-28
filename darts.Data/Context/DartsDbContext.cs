using darts.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace darts.Data.Context;

public class DartsDbContext(DbContextOptions<DartsDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}