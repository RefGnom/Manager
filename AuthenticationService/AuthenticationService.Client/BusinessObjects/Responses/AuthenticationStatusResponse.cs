namespace Manager.AuthenticationService.Client.BusinessObjects.Responses;

public class AuthenticationStatusResponse
{
    public AuthenticationCode AuthenticationCode { get; set; }
    public required string AuthenticationCodeMessage { get; set; }
}