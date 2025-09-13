using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.AppConfiguration.DataBase;

public class NpgDbContextConfigurator(
    IOptions<DataBaseOptions> dataBaseOptions,
    ILogger<NpgDbContextConfigurator> logger
) : DbContextConfiguratorBase(dataBaseOptions, logger)
{
    protected override void InnerConfigureDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder.UseNpgsql(connectionString);
    }
}