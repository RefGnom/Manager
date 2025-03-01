using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Factories;

public interface ITimerDtoFactory
{
    TimerDto CreateArchived(TimerDto forArchiving);
    TimerDto CreateResetTimer(TimerDto forResetting);
    TimerDto CreateDeletedTimer(TimerDto forDeleting);
}