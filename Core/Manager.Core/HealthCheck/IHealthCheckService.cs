using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Manager.Core.HealthCheck;

public interface IHealthCheckService
{
    Task<string> CheckHealthCacheAsync(long utcTicks);
}

public class HealthCheckService(
    IDistributedCache cache,
    ILogger<HealthCheckService> logger
) : IHealthCheckService
{
    private const string CacheHealthCheckKey = "health-check";

    private readonly DistributedCacheEntryOptions distributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
    };

    public async Task<string> CheckHealthCacheAsync(long utcTicks)
    {
        try
        {
            var healthCheckResult = await cache.GetAsync(CacheHealthCheckKey).ConfigureAwait(false);
            if (healthCheckResult != null)
            {
                return Encoding.UTF8.GetString(healthCheckResult);
            }

            var result = (utcTicks % 100).ToString();
            var resultBytes = Encoding.UTF8.GetBytes(result);
            await cache.SetAsync(CacheHealthCheckKey, resultBytes, distributedCacheEntryOptions).ConfigureAwait(false);

            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Cache error in health check");
            return $"Cache error: {e.Message}";
        }
    }
}