using System;

namespace Manager.WorkService.Client.Requests;

public record UpdateWorkRequest(
    Guid Id,
    Guid RecipientId,
    string? Title,
    string? Description,
    DateTime? DeadLineUtc,
    TimeSpan[]? ReminderIntervals
);