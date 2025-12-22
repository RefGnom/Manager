using System;
using System.Threading.RateLimiting;
// using Manager.Core.AppConfiguration;
// using Manager.Core.HostApp;
using Manager.Core.AppConfiguration;
//
// namespace Manager.ApiGateway.Server;
//
// public static class Program
// {
//     public static void Main(string[] args)
//     {
//         var managerHostApp = new ManagerHostApp<HostAppConfigurator>(args);
//         managerHostApp.Run();
//     }
// }

using Manager.Core;
using Manager.Core.Caching;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: ServerProperties("API_GATEWAY_PORT", "manager-api-gateway-service")]

namespace Manager.ApiGateway.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.UseAutoRegistrationForCoreCommon();
        builder.Services.AddDistributedCache(builder.Configuration);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IHealthCheckService, HealthCheckService>();

        builder.Services
            .AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter(
            "ManagerPolicy",
            opt =>
            {
                opt.PermitLimit = 1000; //в течении окна можно сделать 1000 запроса
                opt.Window = TimeSpan.FromSeconds(10); //размер окна в 10 секунд
                opt.QueueProcessingOrder =
                    QueueProcessingOrder
                        .OldestFirst; //Порядок обработки запросов в очереди. Сначала обрабатывается самый старый запрос
                opt.QueueLimit =
                    10000; //Максимальное количество запросов - 1000, которые могут быть поставлены в очередь Если лимит превышен - запросы отклоняются сразу
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



        app.UseMiddleware<CachingMiddleware>();
app.UseRateLimiter();
        app.MapReverseProxy();
        app.MapControllers();
        app.Run();
    }
}