using System;

namespace Manager.WorkService.Client.Requests;

public record CreateWorkRequest(
    Guid RecipientId,
    string Title,
    string? Description = null,
    DateTime? DeadLineUtc = null,
    TimeSpan[]? ReminderIntervals = null
);