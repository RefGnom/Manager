using Manager.Core.HostApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Manager.ApiGateway.Server;

#pragma warning disable CS1591
public class HostAppConfigurator : IHostAppConfigurator
{
    public void ConfigureServiceCollection(IServiceCollection serviceCollection, ILogger logger) { }

    public void ConfigureApplication(IApplicationBuilder applicationBuilder, ILogger logger) { }

    public void ConfigureSwaggerOption(SwaggerGenOptions options) { }
}