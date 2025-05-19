using System;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.UnitTest.Factories;

public interface ITimerSessionDtoTestFactory
{
    TimerSessionDto CreateFromTimes(DateTime startTime, DateTime? endTime);
    TimerSessionDto CreateEmptySession();
}