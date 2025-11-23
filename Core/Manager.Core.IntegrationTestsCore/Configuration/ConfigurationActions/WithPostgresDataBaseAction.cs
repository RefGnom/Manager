using System.Collections.Generic;
using Manager.Core.EFCore;
using Manager.Core.EFCore.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithPostgresDataBaseAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithDataBase;
    public ConfigurationActionType[] ExcludedTypes { get; } = [ConfigurationActionType.WithoutDataBase];

    public void Invoke(ConfigurationActionContext context)
    {
        context.TestContainerBuilder.WithPostgres();
        var configurationDictionary = new Dictionary<string, string?>
        {
            ["DataBaseOptions:ConnectionStringTemplate"] = context.TestContainerBuilder.ConnectionStringTemplate,
            ["DataBaseOptions:Username"] = context.TestContainerBuilder.Username,
            ["DataBaseOptions:Password"] = context.TestContainerBuilder.Password,
        };
        context.ConfigurationManager.AddInMemoryCollection(configurationDictionary);
        context.ServiceCollection
            .ConfigureDb()
            .AddSingleton<IDataContext, DataContext>()
            .AddSingleton<IDbContextConfigurator, NpgTestingDbContextConfigurator>(x
                => new NpgTestingDbContextConfigurator(
                    x.GetRequiredService<IOptions<DataBaseOptions>>(),
                    x.GetRequiredService<ILogger<DbContextConfiguratorBase>>()
                ) { EntitiesAssembly = context.TargetAssembly }
            );
    }
}