using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Caching;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDistributedCache(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "147.45.150.159";
                options.InstanceName = "ManagerRedisCache";
            }
        );
    }
}