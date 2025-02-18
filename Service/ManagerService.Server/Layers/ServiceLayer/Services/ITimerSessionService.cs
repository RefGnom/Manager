using System;
using System.Threading.Tasks;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Services;

public interface ITimerSessionService
{
    Task StartSessionAsync(Guid timerId, DateTime startTime);
    Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId);
    Task StopTimerSessionAsync(Guid id);
}