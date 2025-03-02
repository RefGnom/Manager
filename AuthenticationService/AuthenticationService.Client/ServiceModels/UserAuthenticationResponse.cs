namespace Manager.AuthenticationService.Client.ServiceModels;

public class UserAuthenticationResponse : HttpResponse
{
    public required User User { get; set; }
}