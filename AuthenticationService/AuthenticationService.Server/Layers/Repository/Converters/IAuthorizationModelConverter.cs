using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;

namespace Manager.AuthenticationService.Server.Layers.Repository.Converters;

public interface IAuthorizationModelConverter
{
    AuthorizationModelWithApiKeyHashDbo ToDbo(AuthorizationModelWithApiKeyHashDto authorizationModelWithApiKeyHashDto);
    AuthorizationModelWithApiKeyHashDto ToDto(AuthorizationModelWithApiKeyHashDbo authorizationModelWithApiKeyHashDbo);
    AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDto);
    AuthorizationModelDto ToDto(AuthorizationModelDbo authorizationModelDbo);
}

public class AuthorizationModelConverter : IAuthorizationModelConverter
{
    public AuthorizationModelWithApiKeyHashDbo ToDbo(
        AuthorizationModelWithApiKeyHashDto authorizationModelWithApiKeyHashDto
    ) => new()
    {
        Id = authorizationModelWithApiKeyHashDto.Id,
        ApiKeyHash = authorizationModelWithApiKeyHashDto.ApiKeyHash,
        Owner = authorizationModelWithApiKeyHashDto.Owner,
        AvailableServices = authorizationModelWithApiKeyHashDto.AvailableServices,
        AvailableResources = authorizationModelWithApiKeyHashDto.AvailableResources,
        State = authorizationModelWithApiKeyHashDto.State,
        CreatedUtcTicks = authorizationModelWithApiKeyHashDto.CreatedUtcTicks,
        ExpirationUtcTicks = authorizationModelWithApiKeyHashDto.ExpirationUtcTicks,
    };

    public AuthorizationModelWithApiKeyHashDto ToDto(
        AuthorizationModelWithApiKeyHashDbo authorizationModelWithApiKeyHashDbo
    ) => new(
        authorizationModelWithApiKeyHashDbo.Id,
        authorizationModelWithApiKeyHashDbo.ApiKeyHash,
        authorizationModelWithApiKeyHashDbo.Owner,
        authorizationModelWithApiKeyHashDbo.AvailableServices,
        authorizationModelWithApiKeyHashDbo.AvailableResources,
        authorizationModelWithApiKeyHashDbo.State,
        authorizationModelWithApiKeyHashDbo.CreatedUtcTicks,
        authorizationModelWithApiKeyHashDbo.ExpirationUtcTicks
    );

    public AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDto) => new()
    {
        Id = authorizationModelDto.Id,
        Owner = authorizationModelDto.Owner,
        AvailableServices = authorizationModelDto.AvailableServices,
        AvailableResources = authorizationModelDto.AvailableResources,
        State = authorizationModelDto.State,
        CreatedUtcTicks = authorizationModelDto.CreatedUtcTicks,
        ExpirationUtcTicks = authorizationModelDto.ExpirationUtcTicks,
    };

    public AuthorizationModelDto ToDto(AuthorizationModelDbo authorizationModelDbo) => new(
        authorizationModelDbo.Id,
        authorizationModelDbo.Owner,
        authorizationModelDbo.AvailableServices,
        authorizationModelDbo.AvailableResources,
        authorizationModelDbo.State,
        authorizationModelDbo.CreatedUtcTicks,
        authorizationModelDbo.ExpirationUtcTicks
    );
}