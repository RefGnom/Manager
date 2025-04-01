using System;
using System.Threading.Tasks;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Services;

public interface ITimerSessionService
{
    Task StartAsync(Guid timerId, DateTime startTime);
    Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId);
    Task StopTimerSessionAsync(Guid timerId, DateTime stopTimer);
}