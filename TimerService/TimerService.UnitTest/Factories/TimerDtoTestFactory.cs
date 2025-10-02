using System;
using Manager.Core.Common.Helpers;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.UnitTest.Factories;

public class TimerDtoTestFactory : ITimerDtoTestFactory
{
    public TimerDto CreateEmptyTimer() => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        Name = string.Empty,
        Sessions = [],
        StartTime = DateTime.MinValue,
        PingTimeout = null,
        Status = TimerStatus.Created,
    };

    public TimerDto CreateSameTimer(TimerDto timer) => timer.DeepCopy();
}