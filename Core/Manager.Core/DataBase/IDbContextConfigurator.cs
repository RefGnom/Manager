using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Manager.Core.DataBase;

public interface IDbContextConfigurator
{
    void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder, ILogger logger);
    void ConfigureModel(ModelBuilder modelBuilder);
}