using Manager.Core.AppConfiguration.DataBase;
using Manager.Core.AppConfiguration.DependencyInjection.AutoRegistration;
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
        var startupLogger = builder.AddCustomLogger();

        builder.Services.AddControllers();
        startupLogger.LogInformation("Start configuration service collection");
        builder.Services.AddEndpointsApiExplorer()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseNpg()
            .AddSwaggerGen();
        startupLogger.LogInformation("Service collection configured");

        startupLogger.LogInformation("Build application");
        var app = builder.Build();

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