using Manager.AuthenticationService.Client;
using Manager.Core.Common.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Manager.Core.AppConfiguration.Authentication;

public static class AddApiKeyRequirementExtensions
{
    /// <summary>
    ///     Конфигурация свагера для передачи апи ключа в интерфейсе
    /// </summary>
    public static SwaggerGenOptions ConfigureAuthentication(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            "ApiKey",
            new OpenApiSecurityScheme
            {
                Name = AuthenticationMiddlewareBase.ApiKeyHeaderName,
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Enter your API Key in the header.",
            }
        );
        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey",
                        },
                    },
                    []
                },
            }
        );

        return options;
    }

    /// <summary>
    ///     Конфигурация аутентификации приложения по апи ключу
    /// </summary>
    /// <param name="services">DI collection</param>
    /// <param name="addAuthenticationClient">
    ///     Регистрировать ли апи клиент для сервиса аутентификации.
    ///     Используй true
    /// </param>
    /// <returns>Configured DI collection</returns>
    public static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        bool addAuthenticationClient = true
    )
    {
        services.ConfigureOptionsWithValidation<AuthenticationSetting>();
        if (!addAuthenticationClient)
        {
            return services;
        }

        services.AddSingleton<IAuthenticationServiceApiClientFactory, AuthenticationServiceApiClientFactory>();
        services.AddSingleton<IAuthenticationServiceApiClient>(x
            => x.GetRequiredService<IAuthenticationServiceApiClientFactory>().CreateInternal(
                x.GetRequiredService<IOptions<AuthenticationServiceSetting>>().Value.ApiKey
            )
        );
        services.ConfigureOptionsWithValidation<AuthenticationServiceSetting>();

        return services;
    }

    /// <summary>
    ///     Добавляет мидлвару для аутентификации
    /// </summary>
    public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<AuthenticationMiddleware>();
}