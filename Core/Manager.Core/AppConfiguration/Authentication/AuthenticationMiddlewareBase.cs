using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Manager.Core.Common.HelperObjects.Result;
using Manager.Core.Common.String;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.AppConfiguration.Authentication;

public abstract class AuthenticationMiddlewareBase(
    RequestDelegate next,
    ILogger logger,
    IOptions<AuthenticationSetting> options
)
{
    public const string ApiKeyHeaderName = "X-Api-Key";
    private const int CacheSizeLimit = 1024;
    private const byte CachedValue = 0;

    private readonly MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSize(1)
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

    private readonly AuthenticationSetting setting = options.Value;

    private readonly MemoryCache authenticationCache = new(
        new MemoryCacheOptions
        {
            SizeLimit = CacheSizeLimit,
        }
    );

    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("Start authentication");
        var startTimestamp = Stopwatch.GetTimestamp();
        if (setting.Disabled)
        {
            logger.LogWarning("Authentication is disabled. Use only on development environment.");
            await next.Invoke(context);
            return;
        }

        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await next.Invoke(context);
            return;
        }

        context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey);
        if (apiKey.Count != 1 || apiKey[0].IsNullOrEmpty())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync($"Header {ApiKeyHeaderName} is missing or invalid");
            return;
        }

        var authorizationResourceAttributes = endpoint.Metadata
            .GetOrderedMetadata<AuthorizationResourceAttribute>();
        if (authorizationResourceAttributes.Count <= 0)
        {
            throw new AuthenticationTagMismatchException("Для запроса не настроен ресурс");
        }

        var resource = authorizationResourceAttributes[^1].Resource;
        var startGetAuthResultTimestamp = Stopwatch.GetTimestamp();
        var authenticationResult = await GetCachedAuthenticationResultAsync(setting.Service, resource, apiKey[0]!);
        logger.LogInformation(
            "Authentication got result in {elapsedMs}ms",
            Stopwatch.GetElapsedTime(startGetAuthResultTimestamp).TotalMilliseconds
        );
        if (authenticationResult.IsFailure)
        {
            context.Response.StatusCode = authenticationResult.Error.StatusCode;
            await context.Response.WriteAsync(authenticationResult.Error.Message);
            return;
        }

        logger.LogInformation(
            "Authentication ended in {elapsedMs}ms",
            Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds
        );
        await next.Invoke(context);
    }

    private async Task<Result<(int StatusCode, string Message)>> GetCachedAuthenticationResultAsync(
        string service,
        string resource,
        string apiKey
    )
    {
        authenticationCache.TryGetValue((service, resource, apiKey), out var cachedObject);
        if (cachedObject != null)
        {
            return Result<(int StatusCode, string Message)>.Ok();
        }

        var authenticationResult = await GetAuthenticationResultAsync(service, resource, apiKey);
        if (authenticationResult.IsFailure)
        {
            return authenticationResult;
        }

        authenticationCache.Set((service, resource, apiKey), CachedValue, memoryCacheEntryOptions);
        return authenticationResult;
    }

    protected abstract Task<Result<(int StatusCode, string Message)>> GetAuthenticationResultAsync(
        string service,
        string resource,
        string apiKey
    );
}