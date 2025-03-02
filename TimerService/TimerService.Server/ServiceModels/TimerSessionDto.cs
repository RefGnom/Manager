using System;

namespace Manager.TimerService.Server.ServiceModels;

public class TimerSessionDto
{
    public required Guid Id { get; init; }
    public required Guid TimerId { get; init; }
    public required DateTime StartTime { get; set; }
    public required DateTime? StopTime { get; set; }
    public required bool IsOver { get; set; }
}