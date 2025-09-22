using System;
using Manager.Core.Common.Time;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public interface IAuthorizationModelFactory
{
    AuthorizationModelWithApiKeyDto Create(CreateAuthorizationModelDto createAuthorizationModelDto);
}

public class AuthorizationModelFactory(
    IApiKeyService apiKeyService,
    IDateTimeProvider dateTimeProvider
) : IAuthorizationModelFactory
{
    public AuthorizationModelWithApiKeyDto Create(CreateAuthorizationModelDto createAuthorizationModelDto)
    {
        var authorizationModelId = Guid.NewGuid();
        var apiKey = apiKeyService.CreateApiKey(authorizationModelId);

        return new AuthorizationModelWithApiKeyDto(
            authorizationModelId,
            apiKey,
            createAuthorizationModelDto.Owner,
            createAuthorizationModelDto.AvailableServices,
            createAuthorizationModelDto.AvailableResources,
            AuthorizationModelState.Active,
            dateTimeProvider.UtcTicks,
            createAuthorizationModelDto.ExpirationDateUtc?.Ticks
        );
    }
}