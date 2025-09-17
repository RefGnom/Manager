using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository.Models;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Converters;

public interface IAuthorizationModelConverter
{
    AuthorizationModelDto ToDto(AuthorizationModelDbo authorizationModelDbo);
    AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDbo);
}

public class AuthorizationModelConverter : IAuthorizationModelConverter
{
    public AuthorizationModelDto ToDto(AuthorizationModelDbo authorizationModelDbo)
    {
        return new AuthorizationModelDto(
            authorizationModelDbo.Id,
            authorizationModelDbo.ApiKeyHash,
            authorizationModelDbo.AvailableServices,
            authorizationModelDbo.AvailableResources,
            authorizationModelDbo.CreatedUtcTicks,
            authorizationModelDbo.ExpirationUtcTicks
        );
    }

    public AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDbo)
    {
        return new AuthorizationModelDbo
        {
            Id = authorizationModelDbo.Id,
            ApiKeyHash = authorizationModelDbo.ApiKeyHash,
            AvailableServices = authorizationModelDbo.AvailableServices,
            AvailableResources = authorizationModelDbo.AvailableResources,
            CreatedUtcTicks = authorizationModelDbo.CreatedUtcTicks,
            ExpirationUtcTicks = authorizationModelDbo.ExpirationUtcTicks,
        };
    }
}