using System;
using System.Threading.Tasks;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Services;

public interface ITimerService
{
    Task StartTimerAsync(TimerDto timerDto);
    Task<TimerDto[]> SelectByUserAsync(Guid userId, bool withArchived, bool withDeleted);
    Task StopTimerAsync(Guid userId, string name, DateTime stopTime);
    Task<TimerDto?> FindTimerAsync(Guid userId, string name);
    Task ResetTimerAsync(Guid userId, string name);
    Task DeleteTimerAsync(Guid userId, string name);
    TimeSpan CalculateElapsedTime(TimerDto timerDto);
    Task ArchiveTimerAsync(TimerDto timerToArchiving);
}