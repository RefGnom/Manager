using Manager.TimerService.Client.ServiceModels;
using StartTimerRequest = Manager.ManagerTgClient.Bot.Layers.Api.Requests.StartTimerRequest;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public interface ITimerService
{
    Task StartTimer(StartTimerRequest request);
    Task StopTimer(StopTimerRequest request);
}