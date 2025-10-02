using System;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.UnitTest.Factories;

public class TimerSessionDtoTestFactory : ITimerSessionDtoTestFactory
{
    public TimerSessionDto CreateFromTimes(DateTime startTime, DateTime? stopTime) => new()
    {
        Id = Guid.Empty,
        TimerId = Guid.Empty,
        StartTime = startTime,
        StopTime = stopTime,
        IsOver = stopTime is not null,
    };

    public TimerSessionDto CreateEmptySession() => new()
    {
        Id = Guid.Empty,
        TimerId = Guid.Empty,
        StartTime = DateTime.MinValue,
        StopTime = null,
        IsOver = false,
    };
}