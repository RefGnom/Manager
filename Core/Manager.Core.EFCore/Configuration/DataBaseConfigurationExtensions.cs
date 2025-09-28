using Manager.Core.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.EFCore.Configuration;

public static class DataBaseConfigurationExtensions
{
    public static IServiceCollection UseNpg(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .ConfigureDb()
            .AddSingleton<IDbContextConfigurator, NpgDbContextConfigurator>();
    }

    public static IServiceCollection ConfigureDb(this IServiceCollection serviceCollection)
    {
        serviceCollection.ConfigureOptionsWithValidation<DataBaseOptions>();
        serviceCollection.AddSingleton<IDbContextWrapperFactory, DbContextWrapperFactory>();
        serviceCollection.AddSingleton<IDataContext, DataContext>();
        serviceCollection.AddSingleton(typeof(IDataContext<>), typeof(DataContext<>));

        return serviceCollection;
    }
}