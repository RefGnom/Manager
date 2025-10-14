using System.Threading.Tasks;
using FluentAssertions;
using Manager.AuthenticationService.Client;
using Manager.AuthenticationService.Client.BusinessObjects.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;
using Manager.Core.IntegrationTestsCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using AuthenticationCode = Manager.AuthenticationService.Client.BusinessObjects.AuthenticationCode;

namespace Manager.AuthenticationService.IntegrationTests;

public class AuthenticationClientTest : IntegrationTestBase
{
    private IAuthenticationServiceApiClient AuthenticationServiceApiClient =>
        ServiceProvider.GetRequiredService<IAuthenticationServiceApiClient>();

    private IAuthorizationModelFactory AuthorizationModelFactory =>
        ServiceProvider.GetRequiredService<IAuthorizationModelFactory>();

    private IAuthorizationModelHashService AuthorizationModelHashService =>
        ServiceProvider.GetRequiredService<IAuthorizationModelHashService>();

    private IAuthorizationModelRepository AuthorizationModelRepository =>
        ServiceProvider.GetRequiredService<IAuthorizationModelRepository>();

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

    private async Task<AuthorizationModelWithApiKeyDto> CreateAuthorizationModel(
        string availableService,
        string availableResource
    )
    {
        var authorizationModelWithApiKeyDto = AuthorizationModelFactory.Create(
            "bebe",
            [availableService],
            [availableResource],
            null
        );
        await AuthorizationModelRepository.CreateAsync(
            AuthorizationModelHashService.Hash(authorizationModelWithApiKeyDto)
        );
        return authorizationModelWithApiKeyDto;
    }
}