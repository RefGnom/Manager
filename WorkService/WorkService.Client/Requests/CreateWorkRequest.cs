using System;

namespace Manager.WorkService.Client.Requests;

public record CreateWorkRequest(
    Guid RecipientId,
    string Title,
    string? Description,
    DateTime? DeadLineUtc,
    TimeSpan[]? ReminderIntervals = null
);