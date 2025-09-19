using Manager.AuthenticationService.Server.Layers.Api.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

namespace Manager.AuthenticationService.Server.Layers.Api.Converters;

public interface IAuthorizationConverter
{
    CreateAuthorizationModelDto ToDto(CreateAuthorizationModelRequest createAuthorizationModelRequest);
}

public class AuthorizationConverter : IAuthorizationConverter
{
    public CreateAuthorizationModelDto ToDto(CreateAuthorizationModelRequest createAuthorizationModelRequest)
    {
        return new CreateAuthorizationModelDto(
            createAuthorizationModelRequest.Owner,
            createAuthorizationModelRequest.AvailableServices,
            createAuthorizationModelRequest.AvailableResources,
            createAuthorizationModelRequest.ExpirationDateUtc
        );
    }
}