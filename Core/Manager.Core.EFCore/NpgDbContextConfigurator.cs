using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.EFCore;

public class NpgDbContextConfigurator(
    IOptions<DataBaseOptions> dataBaseOptions,
    ILogger<DbContextConfiguratorBase> logger
) : DbContextConfiguratorBase(dataBaseOptions, logger)
{
    protected override void InnerConfigureDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder.UseNpgsql(connectionString);
    }
}