using System.Text;
using Manager.ApiGateway.Server.Configuration;
using Manager.Core.AppConfiguration;
using Manager.Core.Common.DependencyInjection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.Logging.Configuration;
using Manager.RecipientService.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Manager.Core.HealthCheck;
using Manager.Core.Telemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

SolutionRootEnvironmentVariablesLoader.Load();

[assembly: ServerProperties("API_GATEWAY_PORT", "manager-api-gateway-service")]

namespace Manager.ApiGateway.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
builder.AddCustomLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .UseAutoRegistrationForCoreNetworking()
    .UseAutoRegistrationForCoreCommon()
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
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
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
app.Run();

        app.UseMiddleware<CachingMiddleware>();
        app.UseRateLimiter();
        }
public class JwtAuthOptions
{
    private const string Key = "secret_key_ca938a35-a54e-480a-9ff7-cb4e7f985afb";

    public const string Audience = "ApiGateway.Server";
    public const string Issuer = "ApiGateway.Server";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}
    }
}