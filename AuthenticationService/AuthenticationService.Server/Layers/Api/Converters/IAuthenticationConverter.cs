using Manager.AuthenticationService.Server.Layers.Api.Responses;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Enum;

namespace Manager.AuthenticationService.Server.Layers.Api.Converters;

public interface IAuthenticationConverter
{
    AuthenticationStatusResponse ToResponse(AuthenticationStatusDto authenticationStatusDto);
}

public class AuthenticationConverter : IAuthenticationConverter
{
    public AuthenticationStatusResponse ToResponse(AuthenticationStatusDto authenticationStatusDto)
    {
        return new AuthenticationStatusResponse
        {
            AuthenticationCode = authenticationStatusDto.AuthenticationCode,
            AuthenticationCodeMessage = authenticationStatusDto.AuthenticationCode.GetDescription(),
        };
    }
}