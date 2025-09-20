using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Manager.AuthenticationService.Client.BusinessObjects.Requests;
using Manager.AuthenticationService.Client.BusinessObjects.Responses;
using Manager.AuthenticationService.Client.ServiceModels;

namespace Manager.AuthenticationService.Client;

public class AuthenticationServiceApiClient(
    string apiKey
) : IAuthenticationServiceApiClient
{
    private readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5024/api"),
        DefaultRequestHeaders = { { "X-Api-Key", apiKey } },
    };

    public Task<CreateUserResponse> CreateUserAsync(CreateUserRequest createUserRequest)
    {
        throw new NotImplementedException();
    }

    public Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest userAuthenticationRequest)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponse> UpdateUserInfoAsync(UpdateUserInfoRequest updateUserInfoRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthenticationStatusResponse> GetAuthenticationStatusAsync(
        AuthenticationStatusRequest authenticationStatusRequest
    )
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/authentication/status/{authenticationStatusRequest.Service}/{authenticationStatusRequest.Resource}"
        );
        request.Headers.Add("X-Api-Key-Hash", authenticationStatusRequest.ApiKey);
        var responseMessage = await httpClient.SendAsync(request);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<AuthenticationStatusResponse>()
               ?? throw new Exception("Не смогли десериализовать ответ от сервера");
    }
}