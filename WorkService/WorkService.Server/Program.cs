using Manager.Core.AppConfiguration;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore.Configuration;
using Manager.Core.Logging.Configuration;
using Manager.Core.Telemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

[assembly: ServerProperties("WORK_SERVICE_PORT", "manager-work-service")]

namespace Manager.WorkService.Server;

public class Program
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
            .ConfigureAuthentication()
            .AddSwaggerGen(c => c.ConfigureAuthentication())
            .AddTelemetry<HostAppResourcesFactory>();
        startupLogger.LogInformation("Service collection configured");

        startupLogger.LogInformation("Build application");
        var app = builder.Build();

        app.UseAuthenticationMiddleware();
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