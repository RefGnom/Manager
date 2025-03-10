using System;
using System.Threading.Tasks;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.RepositoryLayer;

public interface ITimerRepository
{
    Task CreateOrUpdateAsync(TimerDto timerDto);
    Task<TimerDto?> FindAsync(Guid userId, string timerName);
    Task<TimerDto[]> SelectByUserAsync(Guid userId);
    Task DeleteAsync(Guid userId);
}