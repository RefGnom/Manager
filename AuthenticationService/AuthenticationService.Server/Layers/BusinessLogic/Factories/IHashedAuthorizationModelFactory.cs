using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;

public interface IHashedAuthorizationModelFactory
{
    AuthorizationModelWithApiKeyHashDto CreateHashedModel(AuthorizationModelDto authorizationModelDto, string hash);
}

public class HashedAuthorizationModelFactory : IHashedAuthorizationModelFactory
{
    public AuthorizationModelWithApiKeyHashDto CreateHashedModel(
        AuthorizationModelDto authorizationModelDto,
        string hash
    ) => new(
        authorizationModelDto.Id,
        hash,
        authorizationModelDto.Owner,
        authorizationModelDto.AvailableServices,
        authorizationModelDto.AvailableResources,
        authorizationModelDto.State,
        authorizationModelDto.CreatedUtcTicks,
        authorizationModelDto.ExpirationUtcTicks
    );
}