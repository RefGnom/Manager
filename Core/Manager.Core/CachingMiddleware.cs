using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Manager.Core;

public class CachingMiddleware(
    RequestDelegate next,
    ILogger logger,
    IDistributedCache cache)
{
    private readonly DistributedCacheEntryOptions distributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
    };

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == "GET")
        {
            logger.LogDebug("Cache invoked");
            var cacheResponse = await cache.GetAsync(context.Request.Path);
            if (cacheResponse is not null)
            {
                logger.LogDebug("Cache hit for {RequestPath}", context.Request.Path);
                context.Response.StatusCode = 200;
                await context.Response.Body.WriteAsync(cacheResponse);
                return;
            }
            logger.LogDebug("Cache miss for {RequestPath}", context.Request.Path);
            await next.Invoke(context);
            if (context.Response.StatusCode == 200)
            {
                var byteBuffer = new byte[context.Request.Body.Length];
                await context.Response.Body.ReadExactlyAsync(byteBuffer, 0, byteBuffer.Length);
                await cache.SetAsync(context.Request.Path, byteBuffer, distributedCacheEntryOptions);
                logger.LogDebug("New cache fot {RequestPath} registered", context.Request.Path);
            }
        }
    }
}

public static class CachingMiddlewareExtensions
{
    public static IApplicationBuilder UseCachingMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<CachingMiddleware>();
}