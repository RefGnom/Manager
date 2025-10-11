namespace Manager.AuthenticationService.Client;

public interface IAuthenticationServiceApiClientFactory
{
    IAuthenticationServiceApiClient Create(string port, string apiKey);
}

public class AuthenticationServiceApiClientFactory : IAuthenticationServiceApiClientFactory
{
    public IAuthenticationServiceApiClient Create(string port, string apiKey)
    {
        var url = $"http://localhost:{port}";
        return new AuthenticationServiceApiClient(url, apiKey);
    }
}