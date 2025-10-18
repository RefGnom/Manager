using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.TimerService.Client;

public class TimerServiceApiClient(
    string url,
    string apiKey
) : ITimerServiceApiClient
{
    private readonly HttpClient httpClient = new()
    {
        BaseAddress = new Uri(url),
        DefaultRequestHeaders = { { "X-Api-Key", apiKey } },
    };

    public async Task<HttpResponse> StartTimerAsync(StartTimerRequest startTimerRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "timers/start")
        {
            Content = JsonContent.Create(startTimerRequest),
        };
        var responseMessage = await httpClient.SendAsync(request);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }

    public async Task<HttpResponse> StopTimerAsync(StopTimerRequest stopTimerRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "timers/stop")
        {
            Content = JsonContent.Create(stopTimerRequest),
        };
        var responseMessage = await httpClient.SendAsync(request);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }

    public async Task<TimerResponse?> FindTimerAsync(TimerRequest timerRequest)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"timers/find?UserId={timerRequest.UserId}&Name={timerRequest.Name}"
        );
        var responseMessage = await httpClient.SendAsync(request);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<TimerResponse>();
    }

    public async Task<UserTimersResponse> SelectUserTimersAsync(UserTimersRequest userTimersRequest)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"timers/selectForUser?UserId={userTimersRequest.UserId}&" +
            $"WithArchived={userTimersRequest.WithArchived}&" +
            $"WithDeleted={userTimersRequest.WithDeleted}"
        );
        var responseMessage = await httpClient.SendAsync(request);
        responseMessage.EnsureSuccessStatusCode();
        return await responseMessage.Content.ReadFromJsonAsync<UserTimersResponse>() ??
               throw new Exception("Не смогли десериализовать ответ от сервера");
    }

    public async Task<HttpResponse> ResetTimerAsync(ResetTimerRequest resetTimerRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "timers/reset")
        {
            Content = JsonContent.Create(resetTimerRequest),
        };
        var responseMessage = await httpClient.SendAsync(request);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }

    public async Task<HttpResponse> DeleteTimerAsync(DeleteTimerRequest deleteTimerRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "timers/delete")
        {
            Content = JsonContent.Create(deleteTimerRequest),
        };
        var responseMessage = await httpClient.SendAsync(request);
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return HttpResponse.Create(responseMessage);
        }

        responseMessage.EnsureSuccessStatusCode();
        return HttpResponse.CreateOk();
    }
}