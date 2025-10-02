using System;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Timers;

public interface ITimerRequestFactory
{
    DeleteTimerRequest CreateDeleteTimerRequest(Guid userId, string timerName);
    ResetTimerRequest CreateResetTimerRequest(Guid userId, string timerName);

    StartTimerRequest CreateStartTimerRequest(
        Guid userId,
        string timerName,
        DateTime startTime,
        TimeSpan? pingTimeout = null
    );

    StopTimerRequest CreateStopTimerRequest(Guid userId, string timerName, DateTime stopTime);
    TimerRequest CreateTimerRequest(Guid userId, string timerName);
    UserTimersRequest CreateUserTimersRequest(Guid userId, bool withArchived, bool withDeleted);
}