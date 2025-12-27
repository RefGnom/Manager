using System.Linq;
using Manager.Core.AppConfiguration;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.BackgroundTasks;
using Manager.Core.Caching;
using Manager.Core.Common.DependencyInjection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore.Configuration;
using Manager.Core.HealthCheck;
using Manager.Core.Logging.Configuration;
using Manager.Core.Networking;
using Manager.Core.Telemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Manager.Core.HostApp;

public class ManagerHostApp<TConfigurator>
    where TConfigurator : IHostAppConfigurator, new()
{
    private readonly TConfigurator customConfigurator = new();
    private readonly WebApplicationBuilder applicationBuilder;
    private readonly ILogger startupLogger;
    private readonly WebApplication application;
    private readonly bool isAuth;

    public ManagerHostApp(string[] args)
    {
        SolutionRootEnvironmentVariablesLoader.Load();

        applicationBuilder = WebApplication.CreateBuilder(args);
        isAuth = args.Contains(HostAppArguments.IsAuth);
        applicationBuilder.AddCustomLogger(OpenTelemetryLogWriteStrategyFactory.CreateForHostApp(applicationBuilder));

        startupLogger = StartupLoggerFactory.CreateStartupLogger();

        ConfigureServiceCollection();

        application = applicationBuilder.Build();

        ConfigureApplication();
    }

    public void Run()
    {
        application.Run();
    }

    private void ConfigureServiceCollection()
    {
        startupLogger.LogInformation("Start configuration service collection");

        applicationBuilder.Services
            .AddEndpointsApiExplorer()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseAutoRegistrationForCoreNetworking()
            .ConfigureOptionsWithValidation<HttpClientOptions>()
            .AddSingleton<IHealthCheckService, HealthCheckService>()
            .UseNpg()
            .ConfigureAuthentication(addAuthenticationClient: !isAuth)
            .AddSwaggerGen(c =>
                {
                    c.ConfigureAuthentication();
                    customConfigurator.ConfigureSwaggerOption(c);
                }
            )
            .AddBackgroundTasks(startupLogger)
            .AddTelemetry<HostAppResourcesFactory>()
            .AddDistributedCache(applicationBuilder.Configuration)
            .AddControllers();
        customConfigurator.ConfigureServiceCollection(applicationBuilder.Services, startupLogger);

        startupLogger.LogInformation("Service collection configured");
    }

    private void ConfigureApplication()
    {
        startupLogger.LogInformation("Start configuration application");

        if (!isAuth)
        {
            startupLogger.LogInformation("Adding auth middleware");
            application.UseAuthenticationMiddleware();
        }

        application.MapControllers();
        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        customConfigurator.ConfigureApplication(application, startupLogger);

        startupLogger.LogInformation("Application configured");
    }
}