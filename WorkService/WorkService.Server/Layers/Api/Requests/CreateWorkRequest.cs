using System;

namespace WorkService.Server.Layers.Api.Requests;

public class CreateWorkRequest
{
    public required Guid RecipientId { get; set; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required DateTime DeadLineUtc { get; init; }
    public required TimeSpan[] ReminderIntervals { get; init; }
}