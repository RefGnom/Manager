using System;
using Microsoft.AspNetCore.Mvc;

namespace Manager.WorkService.Server.Layers.Api.Requests;

public class CreateWorkRequest
{
    [FromRoute]
    public required Guid RecipientId { get; set; }

    [FromBody]
    public required string Title { get; init; }

    [FromBody]
    public string? Description { get; init; }

    [FromBody]
    public DateTime? DeadLineUtc { get; init; }

    [FromBody]
    public TimeSpan[]? ReminderIntervals { get; init; }
}