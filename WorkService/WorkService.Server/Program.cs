using Manager.Core.AppConfiguration.DataBase;
using Manager.Core.AppConfiguration.DependencyInjection.AutoRegistration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Manager.WorkService.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        using var loggerOnConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        Log.Logger = loggerOnConfiguration;
        builder.Host.UseSerilog();

        builder.Services.AddControllers();
        Log.Information("Start configuration service collection");
        builder.Services.AddEndpointsApiExplorer()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseNpg()
            .AddSwaggerGen();
        Log.Information("Service collection configured");


        Log.Information("Build application");
        var app = builder.Build();

        app.MapControllers();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        Log.Information("Application is started");
        app.Run();
    }
}