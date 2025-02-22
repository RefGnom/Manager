using System;
using System.Threading.Tasks;
using Manager.Core.DependencyInjection.LifetimeAttributes;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Services;

[Scoped]
public class TimerSessionService: ITimerSessionService
{
    public Task StartSessionAsync(Guid timerId, DateTime startTime)
    {
        throw new NotImplementedException();
    }

    public Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId)
    {
        throw new NotImplementedException();
    }

    public Task StopTimerSessionAsync(Guid timerId, DateTime stopTimer)
    {
        throw new NotImplementedException();
    }
}