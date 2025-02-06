using ManagerService.Server.Layers.DbLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ManagerDbContext>(
    options => { options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); }
);

var app = builder.Build();

app.Run();