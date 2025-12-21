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
            options.Configuration = string.Join(
                ',',
                redisOptions.Host,
                $"password={redisOptions.Password}",
                $"syncTimeout={redisOptions.TimeoutInMs}"
            );
            options.InstanceName = "ManagerRedisCache";
        }
    );
}