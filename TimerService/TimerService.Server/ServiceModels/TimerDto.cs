using System;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.TimerService.Server.ServiceModels;

public class TimerDto
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; init; }
    public required string Name { get; set; }
    public required DateTime? StartTime { get; set; }
    public required TimeSpan? PingTimeout { get; set; }
    public required TimerSessionDto[] Sessions { get; set; }
    public required TimerStatus Status { get; set; }
}