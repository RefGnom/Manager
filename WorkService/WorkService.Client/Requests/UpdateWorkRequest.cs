using System;

namespace Manager.WorkService.Client.Requests;

public record UpdateWorkRequest(
    Guid Id,
    Guid RecipientId,
    string? Title = null,
    string? Description = null,
    DateTime? DeadLineUtc = null,
    TimeSpan[]? ReminderIntervals= null
);