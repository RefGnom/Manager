using System;
using ManagerService.Client.ServiceModels;

namespace ManagerService.Server.ServiceModels;

public class TimerDto
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public required DateTime StartTime { get; set; }
    public required TimeSpan? PingTimeout { get; set; }
    public required TimerSessionDto[] Sessions { get; set; }
    public required TimerStatus Status { get; init; }
}