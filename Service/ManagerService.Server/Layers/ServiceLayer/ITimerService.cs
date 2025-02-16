using ManagerService.Client.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public interface ITimerService
{
    Task StartTimerAsync(StartTimerRequest request);
    Task<TimerResponse[]> SelectByUserAsync(UserTimersRequest request);
}