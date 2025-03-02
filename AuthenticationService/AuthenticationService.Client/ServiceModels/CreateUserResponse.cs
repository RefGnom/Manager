namespace Manager.AuthenticationService.Client.ServiceModels;

public class CreateUserResponse : HttpResponse
{
    public required User User { get; set; }
}