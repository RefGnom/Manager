using System;
using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Timers;

public class TimerRequestFactory : ITimerRequestFactory
{
    public DeleteTimerRequest CreateDeleteTimerRequest(Guid userId, string timerName)
    {
        return new DeleteTimerRequest
        {
            UserId = userId,
            Name = timerName,
        };
    }

    public ResetTimerRequest CreateResetTimerRequest(Guid userId, string timerName)
    {
        return new ResetTimerRequest
        {
            UserId = userId,
            Name = timerName,
        };
    }

    public StartTimerRequest CreateStartTimerRequest(Guid userId, string timerName, DateTime startTime, TimeSpan? pingTimeout = null)
    {
        return new StartTimerRequest
        {
            UserId = userId,
            Name = timerName,
            StartTime = startTime,
            PingTimeout = pingTimeout,
        };
    }

    public StopTimerRequest CreateStopTimerRequest(Guid userId, string timerName, DateTime stopTime)
    {
        return new StopTimerRequest
        {
            UserId = userId,
            Name = timerName,
            StopTime = stopTime,
        };
    }

    public TimerRequest CreateTimerRequest(Guid userId, string timerName)
    {
        return new TimerRequest
        {
            UserId = userId,
            Name = timerName,
        };
    }

    public UserTimersRequest CreateUserTimersRequest(Guid userId, bool withArchived, bool withDeleted)
    {
        return new UserTimersRequest
        {
            UserId = userId,
            WithArchived = withArchived,
            WithDeleted = withDeleted,
        };
    }
}