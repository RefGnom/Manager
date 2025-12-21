using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Manager.Core;

public class CachingMiddleware(
    RequestDelegate next,
    ILogger logger,
    IDistributedCache cache
)
{
    private readonly DistributedCacheEntryOptions distributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
    };

    public async Task InvokeAsync(HttpContext context)
    {
        if (!IsCacheNeeded(context))
        {
            await next(context);
            return;
        }

        logger.LogInformation("Cache invoked");
        var cacheKey = $"{context.Request.Path.ToString()}?{context.Request.QueryString}";
        if (!TryGetCacheByKey(cacheKey, out var cacheResponse))
        {
            await next.Invoke(context);
            return;
        }

        if (cacheResponse is not null)
        {
            logger.LogDebug("Cache hit for {RequestPath}", cacheKey);
            context.Response.StatusCode = 200;
            await context.Response.Body.WriteAsync(cacheResponse);
            return;
        }

        logger.LogInformation("Cache miss for {RequestPath}", cacheKey);
        var originalBody = context.Response.Body;
        var bodyDummy = new MemoryStream();
        context.Response.Body = bodyDummy;
        await next.Invoke(context);
        if (context.Response.StatusCode is >= 200 and < 300)
        {
            var byteBuffer = new byte[bodyDummy.Length];
            await bodyDummy.ReadExactlyAsync(byteBuffer, 0, byteBuffer.Length);
            bodyDummy.Seek(0, SeekOrigin.Begin);
            await bodyDummy.CopyToAsync(originalBody);
            context.Response.Body = originalBody;
            TrySetCacheByKey(cacheKey, byteBuffer);
            logger.LogInformation("New cache fot {RequestPath} registered", context.Request.Path);
        }
    }

    private bool IsCacheNeeded(HttpContext context)
    {
        return context.Request.Method == "GET";
    }

    private bool TryGetCacheByKey(string cacheKey, out byte[]? response)
    {
        response = null;
        try
        {
            response = cache.Get(cacheKey);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Can't get cache value because cache is unavailable");
            return false;
        }
    }

    private async Task TrySetCacheByKey(string cacheKey, byte[] data)
    {
        try
        {
            await cache.SetAsync(cacheKey, data, distributedCacheEntryOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Can't set cache value because cache is unavailable");
        }
    }
}