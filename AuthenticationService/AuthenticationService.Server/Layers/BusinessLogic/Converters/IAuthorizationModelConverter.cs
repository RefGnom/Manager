using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Converters;

public interface IAuthorizationModelConverter
{
    AuthorizationModelWithApiKeyHashDbo ToDbo(AuthorizationModelWithApiKeyDto authorizationModelWithApiKeyDto);
    AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDto);
    AuthorizationModelDto ToDto(AuthorizationModelDbo authorizationModelDbo);
}

public class AuthorizationModelConverter(
    IApiKeyService apiKeyService
) : IAuthorizationModelConverter
{
    public AuthorizationModelWithApiKeyHashDbo ToDbo(AuthorizationModelWithApiKeyDto authorizationModelWithApiKeyDto) =>
        new()
        {
            Id = authorizationModelWithApiKeyDto.Id,
            ApiKeyHash = apiKeyService.HashApiKey(authorizationModelWithApiKeyDto.ApiKey),
            ApiKeyOwner = authorizationModelWithApiKeyDto.Owner,
            AvailableServices = authorizationModelWithApiKeyDto.AvailableServices,
            AvailableResources = authorizationModelWithApiKeyDto.AvailableResources,
            State = authorizationModelWithApiKeyDto.State,
            CreatedUtcTicks = authorizationModelWithApiKeyDto.CreatedUtcTicks,
            ExpirationUtcTicks = authorizationModelWithApiKeyDto.ExpirationUtcTicks,
        };

    public AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDto) => new()
    {
        Id = authorizationModelDto.Id,
        ApiKeyOwner = authorizationModelDto.Owner,
        AvailableServices = authorizationModelDto.AvailableServices,
        AvailableResources = authorizationModelDto.AvailableResources,
        State = authorizationModelDto.State,
        CreatedUtcTicks = authorizationModelDto.CreatedUtcTicks,
        ExpirationUtcTicks = authorizationModelDto.ExpirationUtcTicks,
    };

    public AuthorizationModelDto ToDto(AuthorizationModelDbo authorizationModelDbo) => new(
        authorizationModelDbo.Id,
        authorizationModelDbo.ApiKeyOwner,
        authorizationModelDbo.AvailableServices,
        authorizationModelDbo.AvailableResources,
        authorizationModelDbo.State,
        authorizationModelDbo.CreatedUtcTicks,
        authorizationModelDbo.ExpirationUtcTicks
    );
}