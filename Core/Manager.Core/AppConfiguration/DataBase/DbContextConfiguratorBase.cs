using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.AppConfiguration.DataBase;

public abstract class DbContextConfiguratorBase(
    IOptions<DataBaseOptions> dataBaseOptions,
    ILogger logger
) : IDbContextConfigurator
{
    private readonly DataBaseOptions dataBaseOptionsValue = dataBaseOptions.Value;
    private Type[] entityTypes = [];
    private bool initialized;

    public void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(
            (_, logLevel) => logLevel >= dataBaseOptionsValue.LogLevel,
            x => logger.Log(x.LogLevel, "{dbLog}", x.ToString())
        );
        if (dataBaseOptionsValue.EnableSensitiveDataLogging)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        InnerConfigureDbContext(optionsBuilder, dataBaseOptionsValue.ConnectionString);
    }

    protected abstract void InnerConfigureDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString);

    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        if (!initialized)
        {
            entityTypes = Assembly.GetEntryAssembly()?.GetExportedTypes()
                .Where(x => x.GetCustomAttribute<TableAttribute>() is not null)
                .ToArray() ?? [];
            initialized = true;
        }

        foreach (var entityType in entityTypes)
        {
            modelBuilder.Entity(entityType);
        }
    }
}