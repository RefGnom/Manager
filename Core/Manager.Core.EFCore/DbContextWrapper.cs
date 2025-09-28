using Microsoft.EntityFrameworkCore;

namespace Manager.Core.EFCore;

internal class DbContextWrapper(
    IDbContextConfigurator dbContextConfigurator
) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        dbContextConfigurator.ConfigureDbContext(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        dbContextConfigurator.ConfigureModel(modelBuilder);
    }
}