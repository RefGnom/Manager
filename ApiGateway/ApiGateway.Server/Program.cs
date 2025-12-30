using System;
using System.Threading.RateLimiting;
using Manager.ApiGateway.Server.Configuration;
using Manager.Core;
using Manager.Core.AppConfiguration;
using Manager.Core.Caching;
using Manager.Core.Common.DependencyInjection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.HealthCheck;
using Manager.Core.Logging.Configuration;
using Manager.RecipientService.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

[assembly: ServerProperties("API_GATEWAY_PORT", "manager-api-gateway-service")]

namespace Manager.ApiGateway.Server;

public class Program
{
    public static void Main(string[] args)
    {
        SolutionRootEnvironmentVariablesLoader.Load();
        var builder = WebApplication.CreateBuilder(args);
        builder.AddCustomLogger();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddDistributedCache(builder.Configuration)
            .UseAutoRegistrationForCoreNetworking()
            .UseAutoRegistrationForCoreCommon()
            .AddSingleton<IHealthCheckService, HealthCheckService>()
            .AddSingleton<IRecipientServiceApiClientFactory, RecipientServiceApiClientFactory>()
            .AddSingleton<IRecipientServiceApiClient>(x =>
                x.GetRequiredService<IRecipientServiceApiClientFactory>().Create("fake key")
            )
            .ConfigureOptionsWithValidation<ApiKeysOptions>()
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JwtAuthOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = JwtAuthOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = JwtAuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                }
            );

        builder.Services
            .AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
            .AddTransforms<ApiKeyTransformProvider>();

        builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(
                    "ManagerPolicy",
                    opt =>
                    {
                        opt.PermitLimit = 1000; // в течении окна можно сделать 1000 запросов
                        opt.Window = TimeSpan.FromSeconds(10); // размер окна в 10 секунд

                        // Порядок обработки запросов в очереди. Сначала обрабатывается самый старый запрос
                        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

                        //Максимальное количество запросов - 1000, которые могут быть поставлены в очередь Если лимит превышен - запросы отклоняются
                        opt.QueueLimit = 1000;
                    }
                );
            }
        );

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication().UseAuthorization();
        app.MapControllers();
        app.MapReverseProxy();
        app.UseMiddleware<CachingMiddleware>();
        app.UseRateLimiter();

        app.Run();
    }
}