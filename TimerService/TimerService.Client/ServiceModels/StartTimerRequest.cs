using System;
using System.Text.Json.Serialization;

namespace Manager.TimerService.Client.ServiceModels;

public record StartTimerRequest(
    [property: JsonIgnore] Guid UserId,
    [property: JsonIgnore] string Name,
    DateTime StartTime,
    TimeSpan? PingTimeout
);