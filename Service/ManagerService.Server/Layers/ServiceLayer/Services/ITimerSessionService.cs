using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Services;

public interface ITimerSessionService
{
    public Task StartSessionAsync(Guid timerId, DateTime startTime);
    public Task<List<TimerSessionDto>> SelectByTimerAsync(Guid timerId);
    public Task StopTimerSessionAsync(Guid id);
}