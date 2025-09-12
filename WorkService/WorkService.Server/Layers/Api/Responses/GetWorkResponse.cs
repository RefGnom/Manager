using System;
using WorkService.Server.Layers.BusinessLogic.Models;

namespace WorkService.Server.Layers.Api.Responses;

public class GetWorkResponse
{
    public required Guid Id { get; set; }
    public required Guid RecipientId { get; set; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required WorkStatus WorkStatus { get; init; }
    public DateTime? DeadLineUtc { get; init; }
    public TimeSpan[]? ReminderIntervals { get; init; }
}