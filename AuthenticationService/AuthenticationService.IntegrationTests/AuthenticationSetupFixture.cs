using System;
using System.Reflection;
using System.Threading.Tasks;
using Manager.AuthenticationService.Client;
using Manager.AuthenticationService.Server;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;
using Manager.AuthenticationService.Server.Layers.Repository.Converters;
using Manager.Core.EFCore;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.AuthenticationService.IntegrationTests;

public class AuthenticationSetupFixture : SetupFixtureBase
{
    private string apiKeyToConnect = null!;

    protected override Assembly TargetTestingAssembly { get; } = typeof(Program).Assembly;

    protected override void CustomizeConfiguration(IConfigurationManager configurationManager)
    {
        configurationManager.AddJsonFile("testsettings.json");
    }

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithLocalServer().WithRealLogger();
    }

    protected override void CustomizeServiceCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IAuthenticationServiceApiClientFactory, AuthenticationServiceApiClientFactory>();
        serviceCollection.AddSingleton<IPasswordHasher<ApiKeyService>, PasswordHasher<ApiKeyService>>();
        serviceCollection.AddSingleton<IAuthenticationServiceApiClient>(x => x
            .GetRequiredService<IAuthenticationServiceApiClientFactory>().Create(
                x.GetRequiredService<IConfiguration>().GetValue<string>("AUTHENTICATION_SERVICE_PORT") ??
                throw new Exception("Authentication service port not configured"),
                apiKeyToConnect
            )
        );
    }

    protected override async Task InnerOneTimeSetup()
    {
        var authorizationModelFactory =
            TestConfiguration.ServiceProvider.GetRequiredService<IAuthorizationModelFactory>();
        var dataContext = TestConfiguration.ServiceProvider.GetRequiredService<IDataContext>();
        var authorizationModelConverter =
            TestConfiguration.ServiceProvider.GetRequiredService<IAuthorizationModelConverter>();

        var authorizationModelWithApiKeyDto = authorizationModelFactory.Create(
            "test owner",
            ["AuthenticationService"],
            ["AuthenticationStatus", "AuthorizationModel"],
            null
        );
        await dataContext.InsertAsync(authorizationModelConverter.ToDbo(authorizationModelWithApiKeyDto));
        apiKeyToConnect = authorizationModelWithApiKeyDto.ApiKey;
    }
}