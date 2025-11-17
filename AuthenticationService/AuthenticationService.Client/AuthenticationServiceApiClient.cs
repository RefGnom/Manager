using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Manager.AuthenticationService.Client.BusinessObjects.Requests;
using Manager.AuthenticationService.Client.BusinessObjects.Responses;
using Manager.Core.Networking;

namespace Manager.AuthenticationService.Client;

public class AuthenticationServiceApiClient(
    IResilientHttpClientFactory httpClientFactory,
    string url,
    string apiKey
) : IAuthenticationServiceApiClient
{
    private readonly IHttpClient httpClient = httpClientFactory.CreateClient(url, apiKey);

    public async Task<AuthenticationStatusResponse> GetAuthenticationStatusAsync(
        AuthenticationStatusRequest authenticationStatusRequest
    )
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/authentication-status/{authenticationStatusRequest.Service}/{authenticationStatusRequest.Resource}"
        );
        request.Headers.Add("X-Caller-Api-Key", authenticationStatusRequest.ApiKey);
        var responseMessage = await httpClient.SendAsync(request, CancellationToken.None);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<AuthenticationStatusResponse>()
            ?? throw new Exception("Не смогли десериализовать ответ от сервера");
    }
}