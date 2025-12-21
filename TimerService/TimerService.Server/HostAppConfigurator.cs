using System;
using System.IO;
using System.Reflection;
using Manager.Core.HostApp;
using Manager.TimerService.Server.Configurators;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Manager.TimerService.Server;

#pragma warning disable CS1591
public class HostAppConfigurator : IHostAppConfigurator
{
    public void ConfigureServiceCollection(IServiceCollection serviceCollection, ILogger logger)
    {
        serviceCollection.AddMapper();
    }

    public void ConfigureApplication(IApplicationBuilder applicationBuilder, ILogger logger) { }

    public void ConfigureSwaggerOption(SwaggerGenOptions options)
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    }
}