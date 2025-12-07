using System;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Timers;

public class TimerRequestFactory : ITimerRequestFactory
{
    public DeleteTimerRequest CreateDeleteTimerRequest(Guid userId, string timerName) => new()
    {
        UserId = userId,
        Name = timerName,
    };

    public ResetTimerRequest CreateResetTimerRequest(Guid userId, string timerName) => new()
    {
        UserId = userId,
        Name = timerName,
    };

    public StartTimerRequest CreateStartTimerRequest(
        Guid userId,
        string timerName,
        DateTime startTime,
        TimeSpan? pingTimeout = null
    ) => new(userId, timerName, startTime, pingTimeout);

    public StopTimerRequest CreateStopTimerRequest(Guid userId, string timerName, DateTime stopTime) => new()
    {
        UserId = userId,
        Name = timerName,
        StopTime = stopTime,
    };

    public TimerRequest CreateTimerRequest(Guid userId, string timerName) => new()
    {
        UserId = userId,
        Name = timerName,
    };

    public UserTimersRequest CreateUserTimersRequest(Guid userId, bool withArchived, bool withDeleted) => new()
    {
        UserId = userId,
        WithArchived = withArchived,
        WithDeleted = withDeleted,
    };
}