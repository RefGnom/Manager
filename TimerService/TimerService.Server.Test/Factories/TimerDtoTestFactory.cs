using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;

namespace TimerService.Server.Test.Factories;

public class TimerDtoTestFactory: ITimerDtoTestFactory
{
    public TimerDto CreateFromSessions(TimerSessionDto[] sessions)
    {
        return new TimerDto()
        {
            Id = Guid.Empty,
            UserId = Guid.Empty,
            Name = string.Empty,
            Sessions = sessions,
            StartTime = DateTime.MinValue,
            PingTimeout = null,
            Status = TimerStatus.Created
        };
    }
}