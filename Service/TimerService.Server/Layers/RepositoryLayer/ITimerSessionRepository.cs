using System;
using System.Threading.Tasks;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.RepositoryLayer;

public interface ITimerSessionRepository
{
    Task CreateAsync(TimerSessionDto timerSessionDto);
    Task UpdateAsync(TimerSessionDto timerSessionDto);
    Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId);
}