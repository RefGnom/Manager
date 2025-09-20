using System;
using Manager.Core.Common.Time;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public interface IAuthorizationModelFactory
{
    AuthorizationModelDto Create(CreateAuthorizationModelDto createAuthorizationModelDto);
}

public class AuthorizationModelFactory(
    IApiKeyService apiKeyService,
    IDateTimeProvider dateTimeProvider
) : IAuthorizationModelFactory
{
    public AuthorizationModelDto Create(CreateAuthorizationModelDto createAuthorizationModelDto)
    {
        var authorizationModelId = Guid.NewGuid();
        var apiKey = apiKeyService.CreateApiKey(authorizationModelId);

        return new AuthorizationModelDto(
            authorizationModelId,
            apiKey,
            createAuthorizationModelDto.Owner,
            createAuthorizationModelDto.AvailableServices,
            createAuthorizationModelDto.AvailableResources,
            dateTimeProvider.UtcTicks,
            createAuthorizationModelDto.ExpirationDateUtc?.Ticks
        );
    }
}