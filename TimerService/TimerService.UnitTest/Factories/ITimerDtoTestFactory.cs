using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.UnitTest.Factories;

public interface ITimerDtoTestFactory
{
    TimerDto CreateEmptyTimer();
    TimerDto CreateSameTimer(TimerDto timer);
}