using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.Enum;
using Manager.Core.Common.HelperObjects.Result;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.AuthenticationService.Server.Layers.Api.Middleware;

public class AuthenticationMiddlewareLocal(
    RequestDelegate next,
    IAuthenticationStatusService authenticationStatusService,
    ILogger<AuthenticationMiddlewareLocal> logger,
    IOptions<AuthenticationSetting> options
) : AuthenticationMiddlewareBase(next, logger, options)
{
    protected override async Task<Result<(int StatusCode, string Message)>> GetAuthenticationResultAsync(
        string service,
        string resource,
        string apiKey
    )
    {
        var authenticationStatusResponse = await authenticationStatusService.GetAsync(apiKey, service, resource);

        if (authenticationStatusResponse.AuthenticationCode is AuthenticationCode.Authenticated)
        {
            return Result<(int StatusCode, string Message)>.Ok();
        }

        var statusCode = authenticationStatusResponse.AuthenticationCode switch
        {
            AuthenticationCode.ApiKeyNotFound or AuthenticationCode.Unknown => StatusCodes.Status401Unauthorized,
            AuthenticationCode.ResourceNotAvailable => StatusCodes.Status403Forbidden,
            AuthenticationCode.ApiKeyRevoked => StatusCodes.Status403Forbidden,
            _ => throw new ArgumentOutOfRangeException(
                $"Неизвестное значение кода аутентификации {authenticationStatusResponse.AuthenticationCode}"
            ),
        };
        return (statusCode, authenticationStatusResponse.AuthenticationCode.GetDescription());
    }
}

public static class AuthenticationMiddlewareLocalExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddlewareLocal(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthenticationMiddlewareLocal>();
    }
}