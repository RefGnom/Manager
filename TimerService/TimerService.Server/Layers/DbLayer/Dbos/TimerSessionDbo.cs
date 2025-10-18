using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manager.TimerService.Server.Layers.DbLayer.Dbos;

[Table("timer_sessions")]
public class TimerSessionDbo
{
    public required Guid Id { get; init; }
    public required Guid TimerId { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime? StopTime { get; init; }
    public required bool IsOver { get; init; }
}