using ManagerService.Server;
using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.Layers.ServiceLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMapper();

builder.Services.AddScoped<ITimerRepository, TimerRepository>();
builder.Services.AddTransient<ITimerService, TimerService>();
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