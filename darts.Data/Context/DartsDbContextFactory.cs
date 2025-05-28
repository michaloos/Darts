using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace darts.Data.Context;

public class DartsDbContextFactory : IDesignTimeDbContextFactory<DartsDbContext>
{
    public DartsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DartsDbContext>();
        
        optionsBuilder.UseSqlite("Data Source=design-time.db");

        return new DartsDbContext(optionsBuilder.Options);
    }
}