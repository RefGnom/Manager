using Manager.AuthenticationService.Server.Layers.Api.Middleware;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.Core.AppConfiguration;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.BackgroundTasks;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore.Configuration;
using Manager.Core.Logging.Configuration;
using Manager.Core.Telemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

[assembly: ServerProperties("AUTHENTICATION_SERVICE_PORT", "manager-authentication-service")]

namespace Manager.AuthenticationService.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        SolutionRootEnvironmentVariablesLoader.Load();

        var builder = WebApplication.CreateBuilder(args);
        builder.AddCustomLogger(OpenTelemetryLogWriteStrategyFactory.CreateForHostApp(builder));
        var startupLogger = StartupLoggerFactory.CreateStartupLogger();

        builder.Services.AddControllers();
        startupLogger.LogInformation("Start configuration service collection");
        builder.Services.AddEndpointsApiExplorer()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseNpg()
            .ConfigureAuthentication(false)
            .AddSwaggerGen(c => c.ConfigureAuthentication())
            .AddBackgroundTasks(startupLogger)
            .AddSingleton<IPasswordHasher<ApiKeyService>, PasswordHasher<ApiKeyService>>()
            .AddTelemetry<HostAppResourcesFactory>();
        startupLogger.LogInformation("Service collection configured");

        startupLogger.LogInformation("Build application");
        var app = builder.Build();

        app.UseAuthenticationMiddlewareLocal();
        app.MapControllers();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        startupLogger.LogInformation("Application is started");
        app.Run();
    }
}