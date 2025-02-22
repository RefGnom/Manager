using Manager.Core.DateTimeProvider;
using ManagerService.Server.Configurators;
using ManagerService.Server.Layers.Api.Converters;
using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.Layers.ServiceLayer.Factories;
using ManagerService.Server.Layers.ServiceLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMapper();

builder.Services.AddScoped<ITimerRepository, TimerRepository>();
builder.Services.AddScoped<ITimerSessionRepository, TimerSessionRepository>();

builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ITimerSessionService, TimerSessionService>();
builder.Services.AddScoped<ITimerService, TimerService>();
builder.Services.AddScoped<ITimerDtoFactory, TimerDtoFactory>();
builder.Services.AddScoped<ITimerSessionHttpModelConverter, TimerSessionHttpModelConverter>();

builder.Services.AddScoped<ITimerHttpModelsConverter, TimerHttpModelsConverter>();

builder.Services.AddScoped<ManagerDbContext>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ManagerDbContext>(
    options => { options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); }
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