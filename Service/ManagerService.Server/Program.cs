using Manager.Core.DateTimeProvider;
using Manager.Core.DependencyInjection;
using ManagerService.Server.Configurators;
using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.Layers.RepositoryLayer.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMapper();

builder.Services.AddSingleton(s =>
    new DbContextOptionsBuilder<ManagerDbContext>()
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).Options
);
builder.Services.AddSingleton<IDbContextFactory<ManagerDbContext>, ManagerDbContextFactory>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.UseAutoRegistrationForCurrentAssembly();

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