using System;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Timers;

public class TimerRequestFactory : ITimerRequestFactory
{
    public StartTimerRequest CreateStartTimerRequest(
        Guid userId,
        string timerName,
        DateTime startTime,
        TimeSpan? pingTimeout = null
    ) => new(userId, timerName, startTime, pingTimeout);

    public StopTimerRequest CreateStopTimerRequest(Guid userId, string timerName, DateTime stopTime) =>
        new(userId, timerName, stopTime);

    public CommonTimerRequest CreateCommonTimerRequest(Guid userId, string timerName) => new(userId, timerName);

    public UserTimersRequest CreateUserTimersRequest(Guid userId, bool withArchived, bool withDeleted) =>
        new(userId, withArchived, withDeleted);
}