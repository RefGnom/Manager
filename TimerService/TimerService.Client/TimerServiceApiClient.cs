using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Manager.Core.Networking;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.TimerService.Client;

public class TimerServiceApiClient(
    IHttpClient httpClient
) : ITimerServiceApiClient
{
    private const string RecipientPath = "api/recipients";

    public async Task<HttpResponse> StartTimerAsync(StartTimerRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{RecipientPath}/{request.UserId}/timers/{request.Name}"
        )
        {
            Content = JsonContent.Create(request),
        };
        var responseMessage = await httpClient.SendAsync(httpRequest);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }

    public async Task<HttpResponse> StopTimerAsync(StopTimerRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{RecipientPath}/{request.UserId}/timers/{request.Name}/stop"
        )
        {
            Content = JsonContent.Create(request),
        };
        var responseMessage = await httpClient.SendAsync(httpRequest);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }

    public async Task<TimerResponse?> FindTimerAsync(CommonTimerRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"{RecipientPath}/{request.UserId}/timers/{request.Name}"
        );
        var responseMessage = await httpClient.SendAsync(httpRequest);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<TimerResponse>();
    }

    public async Task<UserTimersResponse> SelectUserTimersAsync(UserTimersRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"{RecipientPath}/{request.UserId}&" +
            $"WithArchived={request.WithArchived}" +
            $"&WithDeleted={request.WithDeleted}"
        );
        var responseMessage = await httpClient.SendAsync(httpRequest);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<UserTimersResponse>() ??
               throw new Exception("Не смогли десериализовать ответ от сервера");
    }

    public async Task<HttpResponse> ResetTimerAsync(CommonTimerRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            $"{RecipientPath}/{request.UserId}/timers/{request.Name}/reset"
        );
        var responseMessage = await httpClient.SendAsync(httpRequest);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }

    public async Task<HttpResponse> DeleteTimerAsync(CommonTimerRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Delete,
            $"{RecipientPath}/{request.UserId}/timers/{request.Name}"
        );
        var responseMessage = await httpClient.SendAsync(httpRequest);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }
}