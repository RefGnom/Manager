using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

namespace Manager.AuthenticationService.Server.Layers.Api.Responses;

public class AuthenticationStatusResponse
{
    public AuthenticationCode AuthenticationCode { get; set; }
    public required string AuthenticationCodeMessage { get; set; }
}