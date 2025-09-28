using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore.Configuration;
using Manager.Core.Logging.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Manager.WorkService.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddCustomLogger();
        var startupLogger = StartupLoggerFactory.CreateStartupLogger();

        builder.Services.AddControllers();
        startupLogger.LogInformation("Start configuration service collection");
        builder.Services.AddEndpointsApiExplorer()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseNpg()
            .AddApiKeyRequirement()
            .AddSwaggerGen(c => c.AddApiKeyRequirement());
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