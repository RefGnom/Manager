using System;
using System.Threading.Tasks;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public interface ITimerService
{
    Task StartTimerAsync(TimerDto timerDto);
    Task<TimerDto[]> SelectByUserAsync(Guid userId, bool withArchived, bool withDeleted);
}