using System;
using System.Text;
using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.Time;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Manager.Core.AppConfiguration;

[ApiController]
[Route("[controller]")]
public class HealthController(
    IDateTimeProvider dateTimeProvider,
    IDistributedCache cache
) : ControllerBase
{
    private const string CacheHealthCheckKey = "health-check";

    private readonly DistributedCacheEntryOptions distributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10),
    };

    [HttpGet]
    [DisableAuthentication]
    public async Task<IActionResult> Get() => Ok(
        new
        {
            status = "Healthy",
            timestamp = dateTimeProvider.UtcNow,
            cache_result = await GetHealthCheckResultFromCacheAsync(),
        }
    );

    private async Task<string> GetHealthCheckResultFromCacheAsync()
    {
        var healthCheckResult = await cache.GetAsync(CacheHealthCheckKey);
        if (healthCheckResult != null)
        {
            return Encoding.UTF8.GetString(healthCheckResult);
        }

        var result = (dateTimeProvider.UtcTicks % 100).ToString();
        var resultBytes = Encoding.UTF8.GetBytes(result);
        await cache.SetAsync(CacheHealthCheckKey, resultBytes, distributedCacheEntryOptions);

        return result;
    }
}