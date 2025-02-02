using System.Threading.Tasks;
using ManagerService.Client.ServiceModels;

namespace ManagerService.Client;

public interface IManagerServiceApiClient
{
    Task<AuthenticationResponse> AuthenticateUserAsync(AuthenticationRequest authenticationRequest);
    Task<HttpResponse> StartTimerAsync(StartTimerRequest startTimerRequest);
    Task<HttpResponse> StopTimerAsync(StopTimerRequest stopTimerRequest);
    Task<TimerResponse?> FindTimerAsync(TimerRequest timerRequest);
    Task<UserTimersResponse> SelectUserTimers(UserTimersRequest userTimersRequest);
}