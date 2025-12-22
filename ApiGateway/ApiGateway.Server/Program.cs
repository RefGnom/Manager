// using Manager.Core.AppConfiguration;
// using Manager.Core.HostApp;
// using Microsoft.AspNetCore.Builder;
//
// 
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
using Manager.Core.AppConfiguration;
using Manager.Core.Caching;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: ServerProperties("API_GATEWAY_PORT", "It-wanna-docker-container")]

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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<CachingMiddleware>();
        app.MapReverseProxy();
        app.MapControllers();
        app.Run();
    }
}