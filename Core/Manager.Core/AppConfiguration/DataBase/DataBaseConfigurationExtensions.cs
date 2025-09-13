using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Manager.Core.AppConfiguration.DataBase;

public static class DataBaseConfigurationExtensions
{
    public static IHostApplicationBuilder UseNpg(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<DataBaseOptions>(builder.Configuration.GetSection(nameof(DataBaseOptions)));
        builder.Services.AddSingleton<IDbContextConfigurator, NpgDbContextConfigurator>();
        builder.Services.AddSingleton<IDbContextWrapperFactory, DbContextWrapperFactory>();
        builder.Services.AddSingleton<IDataContext, DataContext>();

        return builder;
    }
}