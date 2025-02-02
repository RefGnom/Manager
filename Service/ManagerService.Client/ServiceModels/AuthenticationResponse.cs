namespace ManagerService.Client.ServiceModels;

public class AuthenticationResponse : HttpResponse
{
    public required User User { get; set; }
}