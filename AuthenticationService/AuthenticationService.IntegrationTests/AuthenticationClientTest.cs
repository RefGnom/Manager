using System;
using System.Threading.Tasks;
using FluentAssertions;
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
using AuthenticationCode = Manager.AuthenticationService.Client.BusinessObjects.AuthenticationCode;

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
    public async Task TestGetAuthenticationStatusWithNotExistApiKey()
    {
        // Act
        var authenticationStatusResponse = await AuthenticationServiceApiClient.GetAuthenticationStatusAsync(
            new AuthenticationStatusRequest("Service", "Resource", "ApiKey")
        );

        // Assert
        authenticationStatusResponse.AuthenticationCode.Should().Be(AuthenticationCode.ApiKeyNotFound);
    }

    [Test]
    public async Task TestGetAuthenticationStatusWithExistApiKeyByServiceResource()
    {
        // Arrange
        const string service = "my service";
        const string resource = "my resource";
        var authorizationModelWithApiKeyDto = await CreateAuthorizationModel(service, resource);

        // Act
        var authenticationStatusResponse = await AuthenticationServiceApiClient.GetAuthenticationStatusAsync(
            new AuthenticationStatusRequest(service, resource, authorizationModelWithApiKeyDto.ApiKey)
        );

        // Assert
        authenticationStatusResponse.AuthenticationCode.Should().Be(AuthenticationCode.Authenticated);
    }

    [Test]
    public async Task TestGetAuthenticationStatusWithExistApiKeyButMissingService()
    {
        // Arrange
        const string availableService = "available service";
        const string requestService = "request service";
        const string resource = "my resource";
        var authorizationModelWithApiKeyDto = await CreateAuthorizationModel(availableService, resource);

        // Act
        var authenticationStatusResponse = await AuthenticationServiceApiClient.GetAuthenticationStatusAsync(
            new AuthenticationStatusRequest(requestService, resource, authorizationModelWithApiKeyDto.ApiKey)
        );

        // Assert
        authenticationStatusResponse.AuthenticationCode.Should().Be(AuthenticationCode.ResourceNotAvailable);
    }

    [Test]
    public async Task TestGetAuthenticationStatusWithExistApiKeyButMissingResource()
    {
        // Arrange
        const string service = "my service";
        const string availableResource = "available resource";
        const string requestResource = "request resource";
        var authorizationModelWithApiKeyDto = await CreateAuthorizationModel(service, availableResource);

        // Act
        var authenticationStatusResponse = await AuthenticationServiceApiClient.GetAuthenticationStatusAsync(
            new AuthenticationStatusRequest(service, requestResource, authorizationModelWithApiKeyDto.ApiKey)
        );

        // Assert
        authenticationStatusResponse.AuthenticationCode.Should().Be(AuthenticationCode.ResourceNotAvailable);
    }

    private async Task<AuthorizationModelWithApiKeyDto> CreateAuthorizationModel(string availableService, string availableResource)
    {
        var authorizationModelWithApiKeyDto = AuthorizationModelFactory.Create(
            "bebe",
            [availableService],
            [availableResource],
            null
        );
        await DataContext.InsertAsync(AuthorizationModelConverter.ToDbo(authorizationModelWithApiKeyDto));
        return authorizationModelWithApiKeyDto;
    }
}