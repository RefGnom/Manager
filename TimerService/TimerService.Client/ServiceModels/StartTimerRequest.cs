using System;

namespace Manager.TimerService.Client.ServiceModels;

public record StartTimerRequest(
    Guid UserId,
    string Name,
    DateTime StartTime,
    TimeSpan? PingTimeout
);