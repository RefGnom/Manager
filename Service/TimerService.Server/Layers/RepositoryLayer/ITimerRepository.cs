using System;
using System.Threading.Tasks;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.RepositoryLayer;

public interface ITimerRepository
{
    Task CreateOrUpdateAsync(TimerDto timerDto);
    Task<TimerDto?> FindAsync(Guid userId, string timerName);
    Task<TimerDto[]> SelectByUserAsync(Guid userId);
}