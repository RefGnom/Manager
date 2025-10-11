using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Client;
using Manager.AuthenticationService.Client.BusinessObjects.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Converters;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.IntegrationTestsCore.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.AuthenticationService.IntegrationTests;

public class AuthenticationClientTest : AuthenticationServiceTestBase
{
    private string apiKeyToConnect = null!;

    private IAuthenticationServiceApiClient AuthenticationServiceApiClient =>
        ServiceProvider.GetRequiredService<IAuthenticationServiceApiClient>();

    private IAuthorizationModelFactory AuthorizationModelFactory =>
        ServiceProvider.GetRequiredService<IAuthorizationModelFactory>();

    private IAuthorizationModelConverter AuthorizationModelConverter =>
        ServiceProvider.GetRequiredService<IAuthorizationModelConverter>();

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

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithLocalServer();
    }

    protected override async Task InnerOneTimeSetUp()
    {
        var authorizationModelWithApiKeyDto = AuthorizationModelFactory.Create(
            "test owner",
            ["AuthenticationService"],
            ["AuthenticationStatus", "AuthorizationModel"],
            null
        );
        await DataContext.InsertAsync(AuthorizationModelConverter.ToDbo(authorizationModelWithApiKeyDto));
        apiKeyToConnect = authorizationModelWithApiKeyDto.ApiKey;
    }

    [Test]
    public async Task TestGetAuthenticationStatus()
    {
        await AuthenticationServiceApiClient.GetAuthenticationStatusAsync(
            new AuthenticationStatusRequest("Service", "Resource", "ApiKey")
        );
    }
}