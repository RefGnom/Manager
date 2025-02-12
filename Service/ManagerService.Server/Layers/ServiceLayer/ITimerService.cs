using ManagerService.Client.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public interface ITimerService
{
    Task StartTimerAsync(TimerRequest request);
    Task<TimerResponse[]> SelectByUserAsync(Guid userId);
}