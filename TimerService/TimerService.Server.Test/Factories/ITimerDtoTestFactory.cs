using Manager.TimerService.Server.ServiceModels;

namespace TimerService.Server.Test.Factories;

public interface ITimerDtoTestFactory
{
    public TimerDto CreateFromSessions(TimerSessionDto[] sessions);
    public TimerDto CreateEmptyTimer();
}