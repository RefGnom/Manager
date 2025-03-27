using Manager.TimerService.Server.ServiceModels;

namespace TimerService.Server.Test.Factories;

public interface ITimerDtoTestFactory
{
    public TimerDto CreateEmptyTimer();
    public TimerDto CreateSameTimer(TimerDto timer);
}