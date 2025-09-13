using Microsoft.EntityFrameworkCore;

namespace Manager.Core.AppConfiguration.DataBase;

public interface IDbContextConfigurator
{
    void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder);
    void ConfigureModel(ModelBuilder modelBuilder);
}