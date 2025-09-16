using System;
using System.Threading.Tasks;
using Manager.Core.Common.String;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Manager.Core.AppConfiguration.Authentication;

public class AuthenticationMiddleware(
    RequestDelegate next,
    IAuthorizationService authorizationService,
    IOptions<AuthenticationSetting> options
)
{
    public const string ApiKeyHeaderName = "X-Api-Key";
    private readonly string resource = options.Value.Resource;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next.Invoke(context);
            return;
        }

        context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyValue);
        if (apiKeyValue.Count != 1 || apiKeyValue[0].IsNullOrEmpty())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync($"Header {ApiKeyHeaderName} is missing or invalid");
            return;
        }

        Console.WriteLine(apiKeyValue[0]);
        var authorizationModel = await authorizationService.FindAuthorizationModelAsync(apiKeyValue[0]!, resource);
        if (authorizationModel is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Api key is doesnt exist");
            return;
        }

        if (!authorizationModel.HasAccess)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await next.Invoke(context);
    }
}