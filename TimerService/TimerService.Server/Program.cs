using System;
using System.IO;
using System.Reflection;
using Manager.Core.AppConfiguration;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore;
using Manager.Core.EFCore.Configuration;
using Manager.TimerService.Server.Configurators;
using Manager.TimerService.Server.Layers.DbLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        builder.Services.AddMapper();

        builder.Services.AddDbContext<ManagerDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetSection("DataBaseOptions").Get<DataBaseOptions>()!.ConnectionString
            )
        );

        builder.Services
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseNpg();

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            }
        );

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