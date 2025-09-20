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
    public AuthorizationModelWithApiKeyHashDbo ToDbo(AuthorizationModelWithApiKeyDto authorizationModelWithApiKeyDto)
    {
        return new AuthorizationModelWithApiKeyHashDbo
        {
            Id = authorizationModelWithApiKeyDto.Id,
            ApiKeyHash = apiKeyService.HashApiKey(authorizationModelWithApiKeyDto.ApiKey),
            ApiKeyOwner = authorizationModelWithApiKeyDto.Owner,
            AvailableServices = authorizationModelWithApiKeyDto.AvailableServices,
            AvailableResources = authorizationModelWithApiKeyDto.AvailableResources,
            CreatedUtcTicks = authorizationModelWithApiKeyDto.CreatedUtcTicks,
            ExpirationUtcTicks = authorizationModelWithApiKeyDto.ExpirationUtcTicks,
        };
    }

    public AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDto)
    {
        return new AuthorizationModelDbo
        {
            Id = authorizationModelDto.Id,
            ApiKeyOwner = authorizationModelDto.Owner,
            AvailableServices = authorizationModelDto.AvailableServices,
            AvailableResources = authorizationModelDto.AvailableResources,
            CreatedUtcTicks = authorizationModelDto.CreatedUtcTicks,
            ExpirationUtcTicks = authorizationModelDto.ExpirationUtcTicks,
        };
    }

    public AuthorizationModelDto ToDto(AuthorizationModelDbo authorizationModelDbo)
    {
        return new AuthorizationModelDto(
            authorizationModelDbo.Id,
            authorizationModelDbo.ApiKeyOwner,
            authorizationModelDbo.AvailableServices,
            authorizationModelDbo.AvailableResources,
            authorizationModelDbo.CreatedUtcTicks,
            authorizationModelDbo.ExpirationUtcTicks
        );
    }
}