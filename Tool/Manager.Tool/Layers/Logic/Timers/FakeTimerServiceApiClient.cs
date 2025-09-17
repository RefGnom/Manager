using System;
using System.Net;
using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Timers;

public class FakeTimerServiceApiClient : ITimerServiceApiClient
{
    public Task<HttpResponse> StartTimerAsync(StartTimerRequest startTimerRequest)
    {
        return Task.FromResult(
            new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
            }
        );
    }

    public Task<HttpResponse> StopTimerAsync(StopTimerRequest stopTimerRequest)
    {
        return Task.FromResult(
            new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
            }
        );
    }

    public Task<TimerResponse?> FindTimerAsync(TimerRequest timerRequest)
    {
        return Task.FromResult(CreateTimerResponse())!;
    }

    public Task<UserTimersResponse> SelectUserTimersAsync(UserTimersRequest userTimersRequest)
    {
        return Task.FromResult(
            new UserTimersResponse
            {
                Timers = [CreateTimerResponse(), CreateTimerResponse(), CreateTimerResponse()],
            }
        );
    }

    public Task<HttpResponse> ResetTimerAsync(ResetTimerRequest resetTimerRequest)
    {
        return Task.FromResult(
            new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
            }
        );
    }

    public Task<HttpResponse> DeleteTimerAsync(DeleteTimerRequest deleteTimerRequest)
    {
        return Task.FromResult(
            new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
            }
        );
    }

    private TimerResponse CreateTimerResponse()
    {
        return new TimerResponse
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
}