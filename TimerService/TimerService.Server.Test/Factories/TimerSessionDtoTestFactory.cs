using Manager.TimerService.Server.ServiceModels;

namespace TimerService.Server.Test.Factories;

public class TimerSessionDtoTestFactory: ITimerSessionDtoTestFactory
{
    public TimerSessionDto CreateFromTimes(DateTime startTime, DateTime? stopTime)
    {
        return new TimerSessionDto()
        {
            Id = Guid.Empty,
            TimerId = Guid.Empty,
            StartTime = startTime,
            StopTime = stopTime,
            IsOver = stopTime is not null
        };
    }
}