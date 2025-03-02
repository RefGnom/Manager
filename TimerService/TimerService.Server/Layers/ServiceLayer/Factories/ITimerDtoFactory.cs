using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Factories;

public interface ITimerDtoFactory
{
    TimerDto CreateArchived(TimerDto forArchiving);
    TimerDto CreateResetTimer(TimerDto forResetting);
    TimerDto CreateDeletedTimer(TimerDto forDeleting);
}