// using Manager.Core.AppConfiguration;
// using Manager.Core.HostApp;
// using Microsoft.AspNetCore.Builder;
//
// [assembly: ServerProperties("API_GATEWAY_PORT", "davay randomnoe mi ego ni budem zapuskat")]
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddApplicationPart(typeof(HealthController).Assembly);
builder.Services.UseAutoRegistrationForCoreCommon();
builder.Services.AddDistributedCache(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.Run();