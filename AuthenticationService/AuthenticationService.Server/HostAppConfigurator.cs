using Manager.AuthenticationService.Server.Layers.Api.Middleware;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.Core.HostApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Manager.AuthenticationService.Server;

public class HostAppConfigurator : IHostAppConfigurator
{
    public void ConfigureServiceCollection(IServiceCollection serviceCollection, ILogger logger)
    {
        serviceCollection.AddSingleton<IPasswordHasher<ApiKeyService>, PasswordHasher<ApiKeyService>>();
    }

    public void ConfigureApplication(IApplicationBuilder applicationBuilder, ILogger logger)
    {
        applicationBuilder.UseAuthenticationMiddlewareLocal();
    }

    public void ConfigureSwaggerOption(SwaggerGenOptions options) { }
}