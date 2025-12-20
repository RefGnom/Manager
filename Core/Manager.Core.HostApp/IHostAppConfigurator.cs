using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Manager.Core.HostApp;

public interface IHostAppConfigurator
{
    void ConfigureServiceCollection(IServiceCollection serviceCollection, ILogger logger);
    void ConfigureApplication(IApplicationBuilder applicationBuilder, ILogger logger);
    void ConfigureSwaggerOption(SwaggerGenOptions options);
}