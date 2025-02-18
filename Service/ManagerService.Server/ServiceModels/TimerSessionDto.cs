using System;

namespace ManagerService.Server.ServiceModels;

public class TimerSessionDto
{
    public required Guid Id { get; init; }
    public required Guid TimerId { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime? StopTime { get; init; }
    public required bool IsOver { get; init; }
}