using Microsoft.EntityFrameworkCore;

namespace Manager.Core.EFCore;

public interface IDbContextConfigurator
{
    void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder);
    void ConfigureModel(ModelBuilder modelBuilder);
}