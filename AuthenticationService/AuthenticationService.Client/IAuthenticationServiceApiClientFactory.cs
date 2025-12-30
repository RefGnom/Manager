using Manager.Core.Networking;

namespace Manager.AuthenticationService.Client;

public interface IAuthenticationServiceApiClientFactory
{
    IAuthenticationServiceApiClient Create(string port, string apiKey);
    IAuthenticationServiceApiClient CreateInternal(string apiKey);
}

public class AuthenticationServiceApiClientFactory(
    IResilientHttpClientFactory resilientHttpClientFactory
) : IAuthenticationServiceApiClientFactory
{
    public IAuthenticationServiceApiClient Create(string port, string apiKey) =>
        CreateClient($"http://localhost:{port}", apiKey);

    public IAuthenticationServiceApiClient CreateInternal(string apiKey) =>
        CreateClient($"http://authentication-service:8080", apiKey);

    private AuthenticationServiceApiClient CreateClient(string url, string apiKey)
    {
        var httpClient = resilientHttpClientFactory.CreateClient(url, apiKey);
        return new AuthenticationServiceApiClient(httpClient);
    }
}