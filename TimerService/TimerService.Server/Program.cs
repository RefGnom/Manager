using System;
using System.IO;
using System.Reflection;
using Manager.Core.AppConfiguration;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore.Configuration;
using Manager.TimerService.Server.Configurators;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: ServerProperties("TIMER_SERVICE_PORT", "manager-timer-service")]

namespace Manager.TimerService.Server;

public class Program
{
    public static void Main(string[] args)
    {
        SolutionRootEnvironmentVariablesLoader.Load();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseNpg()
            .AddSwaggerGen(c =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }
            )
            .AddMapper();

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseRouting();
        app.MapControllers();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}