using Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ManagerDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();