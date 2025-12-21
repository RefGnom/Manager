using System;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Timers;

public interface ITimerRequestFactory
{
    StartTimerRequest CreateStartTimerRequest(
        Guid userId,
        string timerName,
        DateTime startTime,
        TimeSpan? pingTimeout = null
    );

    StopTimerRequest CreateStopTimerRequest(Guid userId, string timerName, DateTime stopTime);
    CommonTimerRequest CreateCommonTimerRequest(Guid userId, string timerName);
    UserTimersRequest CreateUserTimersRequest(Guid userId, bool withArchived, bool withDeleted);
}