using System;
using Microsoft.AspNetCore.Mvc;

namespace Manager.WorkService.Server.Layers.Api.Requests;

public class PatchWorkRequest
{
    [FromRoute]
    public required Guid Id { get; set; }

    [FromBody]
    public string? Title { get; init; }

    [FromBody]
    public string? Description { get; init; }

    [FromBody]
    public DateTime? DeadLineUtc { get; init; }

    [FromBody]
    public TimeSpan[]? ReminderIntervals { get; init; }
}