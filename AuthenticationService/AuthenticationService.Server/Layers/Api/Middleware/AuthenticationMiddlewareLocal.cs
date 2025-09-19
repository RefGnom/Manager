using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.Enum;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.AuthenticationService.Server.Layers.Api.Middleware;

public class AuthenticationMiddlewareLocal(
    RequestDelegate next,
    IAuthenticationStatusService authenticationStatusService,
    ILogger logger,
    IOptions<AuthenticationSetting> options
) : AuthenticationMiddlewareBase(next, logger, options)
{
    private readonly RequestDelegate next = next;

    protected override async Task InnerInvokeAsync(HttpContext context, string service, string resource, string apiKey)
    {
        var authenticationStatusResponse = await authenticationStatusService.GetAsync(apiKey, service, resource);

        if (authenticationStatusResponse.AuthenticationCode is AuthenticationCode.Authenticated)
        {
            await next.Invoke(context);
            return;
        }

        context.Response.StatusCode = authenticationStatusResponse.AuthenticationCode switch
        {
            AuthenticationCode.ApiKeyNotFound or AuthenticationCode.Unknown => StatusCodes.Status401Unauthorized,
            AuthenticationCode.ResourceNotAvailable => StatusCodes.Status403Forbidden,
            _ => throw new ArgumentOutOfRangeException(
                $"Неизвестное значение кода аутентификации {authenticationStatusResponse.AuthenticationCode}"
            ),
        };
        await context.Response.WriteAsync(authenticationStatusResponse.AuthenticationCode.GetDescription());
    }
}

public static class AuthenticationMiddlewareLocalExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddlewareLocal(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthenticationMiddlewareLocal>();
    }
}