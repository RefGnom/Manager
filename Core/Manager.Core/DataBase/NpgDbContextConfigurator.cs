using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Manager.Core.DataBase;

public class NpgDbContextConfigurator(
    NpgOptions npgOptions
) : IDbContextConfigurator
{
    public void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder, ILogger logger)
    {
        optionsBuilder.UseNpgsql(npgOptions.ConnectionString);
        optionsBuilder.LogTo(
            (_, logLevel) => logLevel >= npgOptions.LogLevel,
            x => logger.Log(x.LogLevel, "{dbLog}", x.ToString())
        );
        if (npgOptions.EnableSensitiveDataLogging)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity(typeof(DbContextWrapper));
    }
}