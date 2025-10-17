using System;

namespace Manager.WorkService.Client.Requests;

public class UpdateWorkRequest
{
    public required Guid Id { get; set; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? DeadLineUtc { get; init; }
    public TimeSpan[]? ReminderIntervals { get; init; }
}