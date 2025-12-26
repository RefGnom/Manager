using System;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter(
            "ManagerPolicy",
            opt =>
            {
                opt.PermitLimit = 4; //в течении окна можно сделать 4 запроса
                opt.Window = TimeSpan.FromSeconds(12); //размер окна в 12 секунд
                opt.QueueProcessingOrder =
                    QueueProcessingOrder
                        .OldestFirst; //Порядок обработки запросов в очереди. Сначала обрабатывается самый старый запрос
                opt.QueueLimit =
                    2; //Максимальное количество запросов, которые могут быть поставлены в очередь Если лимит превышен - запросы отклоняются сразу
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


app.UseRateLimiter();

app.MapReverseProxy();
app.Run();