using System;
using System.Threading.Tasks;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Services;

public interface ITimerService
{
    Task StartAsync(TimerDto timerDto);
    Task<TimerDto[]> SelectByUserAsync(Guid userId, bool withArchived, bool withDeleted);
    Task StopAsync(Guid userId, string name, DateTime stopTime);
    Task<TimerDto?> FindAsync(Guid userId, string name);
    Task ResetAsync(Guid userId, string name);
    Task DeleteAsync(Guid userId, string name);
    TimeSpan CalculateElapsedTime(TimerDto timerDto);
    Task ArchiveAsync(TimerDto timerToArchiving);
}