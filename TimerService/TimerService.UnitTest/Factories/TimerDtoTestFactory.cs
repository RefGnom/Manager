using System;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.UnitTest.Factories;

public class TimerDtoTestFactory : ITimerDtoTestFactory
{
    public TimerDto CreateEmptyTimer()
    {
        return new TimerDto()
        {
            Id = Guid.Empty,
            UserId = Guid.Empty,
            Name = string.Empty,
            Sessions = null,
            StartTime = DateTime.MinValue,
            PingTimeout = null,
            Status = TimerStatus.Created
        };
    }

    public TimerDto CreateSameTimer(TimerDto timer)
    {
        return new TimerDto()
        {
            Id = timer.Id,
            UserId = timer.UserId,
            Name = timer.Name,
            Sessions = timer.Sessions,
            StartTime = timer.StartTime,
            PingTimeout = timer.PingTimeout,
            Status = TimerStatus.Created
        };
    }
}