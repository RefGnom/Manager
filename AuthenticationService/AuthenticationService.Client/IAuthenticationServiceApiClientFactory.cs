namespace Manager.AuthenticationService.Client;

public interface IAuthenticationServiceApiClientFactory
{
    IAuthenticationServiceApiClient Create(string apiKey);
}

public class AuthenticationServiceApiClientFactory : IAuthenticationServiceApiClientFactory
{
    public IAuthenticationServiceApiClient Create(string apiKey)
    {
        return new AuthenticationServiceApiClient(apiKey);
    }
}