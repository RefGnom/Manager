using Manager.Core.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.EFCore.Configuration;

public static class DataBaseConfigurationExtensions
{
    /// <summary>
    ///     Добавляет работу с Postgresql
    /// </summary>
    public static IServiceCollection UseNpg(this IServiceCollection serviceCollection) => serviceCollection
        .ConfigureDb()
        .AddSingleton<IDbContextConfigurator, NpgDbContextConfigurator>();

    /// <summary>
    ///     Регистрирует зависимости для работы с EFCore с помощью IDataContext
    /// </summary>
    public static IServiceCollection ConfigureDb(this IServiceCollection serviceCollection)
    {
        serviceCollection.ConfigureOptionsWithValidation<DataBaseOptions>();
        serviceCollection.AddSingleton<IDbContextWrapperFactory, DbContextWrapperFactory>();
        serviceCollection.AddSingleton<IDataContext, DataContext>();
        serviceCollection.AddSingleton(typeof(IDataContext<>), typeof(DataContext<>));

        return serviceCollection;
    }
}