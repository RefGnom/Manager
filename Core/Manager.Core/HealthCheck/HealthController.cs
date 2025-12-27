using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.Time;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Core.HealthCheck;

[ApiController]
[Route("[controller]")]
public class HealthController(
    IDateTimeProvider dateTimeProvider,
    IHealthCheckService healthCheckService
) : ControllerBase
{
    [HttpGet]
    [DisableAuthentication]
    public async Task<IActionResult> Get()
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        var timestamp = dateTimeProvider.UtcNow;
        var cacheHealth = await healthCheckService.CheckHealthCacheAsync(timestamp.Ticks).ConfigureAwait(false);

        var endTimestamp = Stopwatch.GetTimestamp();
        return Ok(
            new
            {
                status = "Healthy",
                timestamp,
                cache_result = cacheHealth,
                //elapsed_ms = (endTimestamp - startTimestamp) / TimeSpan.TicksPerMillisecond,
                elapsed_ms = TimeSpan.FromTicks(endTimestamp - startTimestamp).TotalMilliseconds,
            }
        );
    }
}