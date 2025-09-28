using System.Reflection;
using Manager.Core.AppConfiguration.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.IntegrationTestsCore;

public class NpgTestingDbContextConfigurator(
    IOptions<DataBaseOptions> dataBaseOptions,
    ILogger<DbContextConfiguratorBase> logger
) : DbContextConfiguratorBase(dataBaseOptions, logger)
{
    public Assembly? EntitiesAssembly { get; set; }

    protected override void InnerConfigureDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override Assembly? GetEntitiesAssembly()
    {
        return EntitiesAssembly;
    }
}