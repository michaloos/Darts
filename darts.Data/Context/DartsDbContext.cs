using darts.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace darts.Data.Context;

public class DartsDbContext(DbContextOptions<DartsDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}

public class DartsDbContextFactory : IDesignTimeDbContextFactory<DartsDbContext>
{
    public DartsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DartsDbContext>();
        
        optionsBuilder.UseSqlite("Data Source=design-time.db");

        return new DartsDbContext(optionsBuilder.Options);
    }
}