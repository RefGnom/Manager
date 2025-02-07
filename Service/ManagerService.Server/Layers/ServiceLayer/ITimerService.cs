using ManagerService.Client.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public interface ITimerService
{
    Task StartTimerAsync(User user, string name); // todo: добавить остальные параметры
}