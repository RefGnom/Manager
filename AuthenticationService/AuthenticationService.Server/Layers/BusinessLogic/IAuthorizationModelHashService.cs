using Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthorizationModelHashService
{
    AuthorizationModelWithApiKeyHashDto Hash(AuthorizationModelWithApiKeyDto authorizationModelWithApiKeyDto);
}

public class AuthorizationModelHashService(
    IApiKeyService apiKeyService,
    IHashedAuthorizationModelFactory hashedAuthorizationModelFactory
) : IAuthorizationModelHashService
{
    public AuthorizationModelWithApiKeyHashDto Hash(AuthorizationModelWithApiKeyDto authorizationModelWithApiKeyDto) =>
        hashedAuthorizationModelFactory.CreateHashedModel(
            authorizationModelWithApiKeyDto,
            apiKeyService.HashApiKey(authorizationModelWithApiKeyDto.ApiKey)
        );
}