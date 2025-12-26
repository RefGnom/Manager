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
                opt.PermitLimit = 1000; //в течении окна можно сделать 1000 запроса
                opt.Window = TimeSpan.FromSeconds(10); //размер окна в 10 секунд
                opt.QueueProcessingOrder =
                    QueueProcessingOrder
                        .OldestFirst; //Порядок обработки запросов в очереди. Сначала обрабатывается самый старый запрос
                opt.QueueLimit =
                    10000; //Максимальное количество запросов - 1000, которые могут быть поставлены в очередь Если лимит превышен - запросы отклоняются сразу
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