using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Manager.Core.ApiClientBase;
using Manager.RecipientService.Client.BusinessObjects.Requests;
using Manager.RecipientService.Client.BusinessObjects.Responses;

namespace Manager.RecipientService.Client;

public class RecipientServiceApiClient(
    string url,
    string apiKey
) : IRecipientServiceApiClient
{
    private readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri(url),
        DefaultRequestHeaders = { { "X-Api-Key", apiKey } },
    };

    public async Task<HttpResult> CreateRecipientAccountAsync(CreateRecipientAccountRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "recipient-account")
        {
            Content = JsonContent.Create(request),
        };
        return await httpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResult<RecipientAccountResponse>> GetRecipientAccountAsync(Guid recipientId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"recipient-account/{recipientId}");
        return await httpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResult> DeleteRecipientAccountAsync(Guid recipientId)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, $"recipient-account/{recipientId}");
        return await httpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResult> UpdateRecipientAccountAsync(PatchRecipientAccountRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Patch, "recipient-account");
        return await httpClient.SendAsync(httpRequest);
    }

    public async Task<HttpResult<RecipientAuthorizationResponse>> GetRecipientAuthorizationAsync(
        RecipientAuthorizationRequest request
    )
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"recipient-authorization" +
            $"?RecipientId={request.RecipientId}" +
            $"&RequestedService={request.RequestedService}" +
            $"&RequestedResource={request.RequestedResource}"
        );
        return await httpClient.SendAsync(httpRequest);
    }
}