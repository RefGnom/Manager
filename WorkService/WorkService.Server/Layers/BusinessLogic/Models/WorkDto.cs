using System;

namespace WorkService.Server.Layers.BusinessLogic.Models;

public record WorkDto(
    Guid Id,
    Guid RecipientId,
    string Title,
    string Description,
    WorkStatus WorkStatus,
    DateTime CreatedUtc,
    DateTime DeadLineUtc,
    TimeSpan[] ReminderIntervals
);