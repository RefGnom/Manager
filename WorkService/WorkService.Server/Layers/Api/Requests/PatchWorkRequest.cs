﻿using System;

namespace Manager.WorkService.Server.Layers.Api.Requests;

public class PatchWorkRequest
{
    public required Guid Id { get; set; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? DeadLineUtc { get; init; }
    public TimeSpan[]? ReminderIntervals { get; init; }
}