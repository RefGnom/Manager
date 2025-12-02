using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Manager.Core.Networking;
using Manager.WorkService.Client.Requests;
using Manager.WorkService.Client.Responses;
using Microsoft.Extensions.Logging;

namespace Manager.WorkService.Client;

public class WorkServiceApiClient(
    IHttpClient httpClient,
    ILogger<WorkServiceApiClient> logger
) : IWorkServiceApiClient
{
    private const string WorksBasePath = "api/works/";

    public async Task<Guid> CreateWorkAsync(CreateWorkRequest createWorkRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, WorksBasePath)
        {
            Content = JsonContent.Create(createWorkRequest),
        };
        var responseMessage = await httpClient.SendAsync(request);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task<GetWorkResponse?> FindWorkAsync(Guid workId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{WorksBasePath}{workId}");
        var responseMessage = await httpClient.SendAsync(request);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<GetWorkResponse>();
    }

    public Task<GetWorkResponse[]> SelectWorksAsync(Guid recipientId) =>
        SelectBaseAsync(recipientId);

    public Task<GetWorkResponse[]> SelectActualWorksAsync(Guid recipientId) =>
        SelectBaseAsync(recipientId, "actual");

    public Task<GetWorkResponse[]> SelectWorksForReminderAsync(Guid recipientId) =>
        SelectBaseAsync(recipientId, "ready-for-reminder");

    public Task<GetWorkResponse[]> SelectExpiredWorksAsync(Guid recipientId) =>
        SelectBaseAsync(recipientId, "expired");

    public Task<GetWorkResponse[]> SelectDeletedWorksAsync(Guid recipientId) =>
        SelectBaseAsync(recipientId, "deleted");

    public Task<GetWorkResponse[]> SelectCompletedWorksAsync(Guid recipientId) =>
        SelectBaseAsync(recipientId, "completed");

    public async Task UpdateWorkAsync(UpdateWorkRequest updateWorkRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, WorksBasePath)
        {
            Content = JsonContent.Create(updateWorkRequest),
        };
        var responseMessage = await httpClient.SendAsync(request);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task DeleteWorkAsync(Guid workId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{WorksBasePath}{workId}");
        var responseMessage = await httpClient.SendAsync(request);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogInformation("Work {workId} already deleted", workId);
        }

        responseMessage.EnsureSuccessStatusCode();
    }

    private async Task<GetWorkResponse[]> SelectBaseAsync(Guid recipientId, string concretePath = "")
    {
        var path = $"api/recipients/{recipientId}/works/{concretePath}";
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        var responseMessage = await httpClient.SendAsync(request);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<GetWorkResponse[]>() ??
               throw new Exception("Не смогли десериализовать ответ от сервера");
    }
}