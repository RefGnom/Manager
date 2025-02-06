﻿using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.RepositoryLayer;

public interface ITimerSessionRepository
{
    Task CreateOrUpdate(TimerSessionDto timerSessionDto);
    Task<TimerSessionDto[]> SelectByTimer(Guid timerId);
}