using Manager.Core.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.AppConfiguration.DataBase;

public static class DataBaseConfigurationExtensions
{
    public static IServiceCollection UseNpg(this IServiceCollection serviceCollection)
    {
        serviceCollection.ConfigureOptionsWithValidation<DataBaseOptions>();
        serviceCollection.AddSingleton<IDbContextConfigurator, NpgDbContextConfigurator>();
        serviceCollection.AddSingleton<IDbContextWrapperFactory, DbContextWrapperFactory>();
        serviceCollection.AddSingleton<IDataContext, DataContext>();

        return serviceCollection;
    }
}