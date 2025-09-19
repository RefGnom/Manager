using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Client;
using Manager.AuthenticationService.Client.BusinessObjects;
using Manager.AuthenticationService.Client.BusinessObjects.Requests;
using Manager.Core.Common.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.AppConfiguration.Authentication;

public class AuthenticationMiddleware(
    RequestDelegate next,
    IAuthenticationServiceApiClient authenticationServiceApiClient,
    ILogger<AuthenticationMiddleware> logger,
    IOptions<AuthenticationSetting> options
) : AuthenticationMiddlewareBase(next, logger, options)
{
    private readonly RequestDelegate next = next;

    protected override async Task InnerInvokeAsync(HttpContext context, string service, string resource, string apiKey)
    {
        var authenticationStatusResponse = await authenticationServiceApiClient.GetAuthenticationStatusAsync(
            new AuthenticationStatusRequest(service, resource, apiKey)
        );

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