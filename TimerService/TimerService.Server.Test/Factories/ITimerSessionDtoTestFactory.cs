using Manager.TimerService.Server.ServiceModels;

namespace TimerService.Server.Test.Factories;

public interface ITimerSessionDtoTestFactory
{
    TimerSessionDto CreateFromTimes(DateTime startTime, DateTime? endTime);
    TimerSessionDto CreateEmptySession();
}