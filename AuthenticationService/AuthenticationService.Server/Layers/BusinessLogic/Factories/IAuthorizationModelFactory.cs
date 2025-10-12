using System;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Time;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;

public interface IAuthorizationModelFactory
{
    AuthorizationModelWithApiKeyDto Create(CreateAuthorizationModelDto createAuthorizationModelDto);

    AuthorizationModelWithApiKeyDto Create(
        string owner,
        string[] availableServices,
        string[] availableResources,
        long? expirationUtcTicks
    );
}

public class AuthorizationModelFactory(
    IApiKeyService apiKeyService,
    IDateTimeProvider dateTimeProvider
) : IAuthorizationModelFactory
{
    public AuthorizationModelWithApiKeyDto Create(CreateAuthorizationModelDto createAuthorizationModelDto) => Create(
        createAuthorizationModelDto.Owner,
        createAuthorizationModelDto.AvailableServices,
        createAuthorizationModelDto.AvailableResources,
        createAuthorizationModelDto.ExpirationDateUtc?.Ticks
    );

    public AuthorizationModelWithApiKeyDto Create(
        string owner,
        string[] availableServices,
        string[] availableResources,
        long? expirationUtcTicks
    )
    {
        var authorizationModelId = Guid.NewGuid();
        var apiKey = apiKeyService.CreateApiKey(authorizationModelId);

        return new AuthorizationModelWithApiKeyDto(
            authorizationModelId,
            apiKey,
            owner,
            availableServices,
            availableResources,
            AuthorizationModelState.Active,
            dateTimeProvider.UtcTicks,
            expirationUtcTicks
        );
    }
}