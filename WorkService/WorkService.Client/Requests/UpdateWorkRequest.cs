using System;
using System.Text.Json.Serialization;

namespace Manager.WorkService.Client.Requests;

public record UpdateWorkRequest(
    [property: JsonIgnore] Guid Id,
    [property: JsonIgnore] Guid RecipientId,
    string? Title,
    string? Description,
    DateTime? DeadLineUtc,
    TimeSpan[]? ReminderIntervals
);