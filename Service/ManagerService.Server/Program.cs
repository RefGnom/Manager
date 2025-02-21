// using Manager.Core.DateTimeProvider;
using Manager.Core.DependencyInjection;
using ManagerService.Server.Configurators;
using ManagerService.Server.Layers.DbLayer;
// using ManagerService.Server.Layers.Api.Converters;
// using ManagerService.Server.Layers.DbLayer;
// using ManagerService.Server.Layers.RepositoryLayer;
// using ManagerService.Server.Layers.RepositoryLayer.Factories;
// using ManagerService.Server.Layers.RepositoryLayer.Repositories;
// using ManagerService.Server.Layers.ServiceLayer.Factories;
// using ManagerService.Server.Layers.ServiceLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMapper();

// builder.Services.AddSingleton<ITimerRepository, TimerRepository>();
// builder.Services.AddSingleton<ITimerSessionRepository, TimerSessionRepository>();
//
// builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
// builder.Services.AddSingleton<ITimerSessionService, TimerSessionService>();
// builder.Services.AddSingleton<ITimerService, TimerService>();
// builder.Services.AddSingleton<ITimerDtoFactory, TimerDtoFactory>();
// builder.Services.AddSingleton<ITimerSessionHttpModelConverter, TimerSessionHttpModelConverter>();


builder.Services.AddSingleton(s =>
    new DbContextOptionsBuilder<ManagerDbContext>()
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).Options
);

builder.Services.AddSingleton<ManagerDbContext>();

// builder.Services.AddSingleton<IDbContextFactory<ManagerDbContext>, ManagerDbContextFactory>();
// builder.Services.AddSingleton<ITimerHttpModelsConverter, TimerHttpModelsConverter>();

AutoRegistrationExtensions.UseAutoRegistrationForCurrentAssembly(builder.Services);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();