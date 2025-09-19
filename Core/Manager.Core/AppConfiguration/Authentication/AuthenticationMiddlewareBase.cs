using System.Security.Cryptography;
using System.Threading.Tasks;
using Manager.Core.Common.String;
using Microsoft.AspNetCore.Http;
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

    private readonly AuthenticationSetting setting = options.Value;

    public async Task InvokeAsync(HttpContext context)
    {
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
        await InnerInvokeAsync(context, setting.Service, resource, apiKey[0]!);
    }

    protected abstract Task InnerInvokeAsync(HttpContext context, string service, string resource, string apiKey);
}