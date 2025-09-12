using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Manager.Core.DataBase;

internal class DbContextWrapper(IDbContextConfigurator dbContextConfigurator, ILogger logger) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        dbContextConfigurator.ConfigureDbContext(optionsBuilder, logger);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        dbContextConfigurator.ConfigureModel(modelBuilder);
    }
}