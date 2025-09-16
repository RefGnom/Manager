using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Manager.Core.AppConfiguration.Authentication;

public static class AddApiKeyRequirementExtensions
{
    public static SwaggerGenOptions AddApiKeyRequirement(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            "ApiKey",
            new OpenApiSecurityScheme
            {
                Name = AuthenticationMiddleware.ApiKeyHeaderName,
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

    public static IServiceCollection AddApiKeyRequirement(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationService, AuthorizationService>();
        services.ConfigureOptionsWithValidation<AuthenticationSetting>();

        return services;
    }

    public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthenticationMiddleware>();
    }
}