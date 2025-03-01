using System;
using System.Threading.Tasks;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.RepositoryLayer;

public interface ITimerSessionRepository
{
    Task CreateAsync(TimerSessionDto timerSessionDto);
    Task UpdateAsync(TimerSessionDto timerSessionDto);
    Task<TimerSessionDto[]> SelectByTimer(Guid timerId);
    Task<TimerDto?> LastOrDefaultForTimerAsync(Guid timerId);
}