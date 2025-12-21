using System;
using System.Text.Json.Serialization;

namespace Manager.WorkService.Server.Layers.Api.Requests;

public class PatchWorkRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid RecipientId { get; set; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public DateTime? DeadLineUtc { get; init; }

    public TimeSpan[]? ReminderIntervals { get; init; }
}