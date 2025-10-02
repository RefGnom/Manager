using System;
using System.Net;
using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Timers;

public class FakeTimerServiceApiClient : ITimerServiceApiClient
{
    public Task<HttpResponse> StartTimerAsync(StartTimerRequest startTimerRequest) => Task.FromResult(
        new HttpResponse
        {
            StatusCode = HttpStatusCode.OK,
        }
    );

    public Task<HttpResponse> StopTimerAsync(StopTimerRequest stopTimerRequest) => Task.FromResult(
        new HttpResponse
        {
            StatusCode = HttpStatusCode.OK,
        }
    );

    public Task<TimerResponse?> FindTimerAsync(TimerRequest timerRequest) => Task.FromResult(CreateTimerResponse())!;

    public Task<UserTimersResponse> SelectUserTimersAsync(UserTimersRequest userTimersRequest) => Task.FromResult(
        new UserTimersResponse
        {
            Timers = [CreateTimerResponse(), CreateTimerResponse(), CreateTimerResponse()],
        }
    );

    public Task<HttpResponse> ResetTimerAsync(ResetTimerRequest resetTimerRequest) => Task.FromResult(
        new HttpResponse
        {
            StatusCode = HttpStatusCode.OK,
        }
    );

    public Task<HttpResponse> DeleteTimerAsync(DeleteTimerRequest deleteTimerRequest) => Task.FromResult(
        new HttpResponse
        {
            StatusCode = HttpStatusCode.OK,
        }
    );

    private TimerResponse CreateTimerResponse() => new()
    {
        Name = "default",
        StartTime = DateTime.Today,
        ElapsedTime = TimeSpan.FromMinutes(400),
        PingTimeout = null,
        Sessions =
        [
            new TimerSessionResponse
            {
                StartTime = DateTime.Today,
                StopTime = null,
                IsOver = false,
            },
        ],
        TimerStatus = TimerStatus.Started,
    };
}