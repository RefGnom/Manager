using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Converters;

public interface IAuthorizationModelConverter
{
    AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDbo);
}

public class AuthorizationModelConverter(IApiKeyService apiKeyService) : IAuthorizationModelConverter
{
    public AuthorizationModelDbo ToDbo(AuthorizationModelDto authorizationModelDbo)
    {
        return new AuthorizationModelDbo
        {
            Id = authorizationModelDbo.Id,
            ApiKeyHash = apiKeyService.HashApiKey(authorizationModelDbo.ApiKey),
            ApiKeyOwner = authorizationModelDbo.Owner,
            AvailableServices = authorizationModelDbo.AvailableServices,
            AvailableResources = authorizationModelDbo.AvailableResources,
            CreatedUtcTicks = authorizationModelDbo.CreatedUtcTicks,
            ExpirationUtcTicks = authorizationModelDbo.ExpirationUtcTicks,
        };
    }
}