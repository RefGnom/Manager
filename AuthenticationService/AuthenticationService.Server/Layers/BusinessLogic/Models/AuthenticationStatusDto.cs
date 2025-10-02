namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record AuthenticationStatusDto(
    AuthenticationCode AuthenticationCode
)
{
    public static implicit operator AuthenticationStatusDto(AuthenticationCode code) => new(code);
}