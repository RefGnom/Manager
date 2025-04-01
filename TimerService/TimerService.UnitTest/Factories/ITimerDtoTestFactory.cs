using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.UnitTest.Factories;

public interface ITimerDtoTestFactory
{
    public TimerDto CreateEmptyTimer();
    public TimerDto CreateSameTimer(TimerDto timer);
}