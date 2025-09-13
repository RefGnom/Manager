using Manager.Core.AppConfiguration.DataBase;
using Manager.Core.AppConfiguration.DependencyInjection.AutoRegistration;
using Manager.Core.DateTimeProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Manager.WorkService.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.UseAutoRegistrationForCurrentAssembly();
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        builder.UseNpg();

        var app = builder.Build();

        app.MapControllers();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}