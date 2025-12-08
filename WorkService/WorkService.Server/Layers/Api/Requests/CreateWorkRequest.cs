using System;

namespace Manager.WorkService.Server.Layers.Api.Requests;

public class CreateWorkRequest
{
    public required Guid RecipientId { get; set; }

    public required string Title { get; init; }

    public string? Description { get; init; }

    public DateTime? DeadLineUtc { get; init; }

    public TimeSpan[]? ReminderIntervals { get; init; }
}