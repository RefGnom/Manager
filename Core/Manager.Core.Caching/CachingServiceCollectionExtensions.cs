using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Caching;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDistributedCache(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    ) => serviceCollection.AddStackExchangeRedisCache(options =>
        {
            var redisOptions = new RedisOptions();
            configuration.Bind("RedisOptions", redisOptions);
            options.Configuration = $"147.45.150.159,password={redisOptions.Password}";
            options.InstanceName = "ManagerRedisCache";
        }
    );
}