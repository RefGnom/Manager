using Manager.Core.Networking;

namespace Manager.AuthenticationService.Client;

public interface IAuthenticationServiceApiClientFactory
{
    IAuthenticationServiceApiClient Create(string port, string apiKey);
}

public class AuthenticationServiceApiClientFactory(
    IResilientHttpClientFactory resilientHttpClientFactory
) : IAuthenticationServiceApiClientFactory
{
    public IAuthenticationServiceApiClient Create(string port, string apiKey)
    {
        var url = $"http://localhost:{port}";
        return new AuthenticationServiceApiClient(resilientHttpClientFactory, url, apiKey);
    }
}