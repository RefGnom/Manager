using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Manager.WorkService.Server.Layers.BusinessLogic.Models;

namespace WorkService.Server.Layers.Repository.Dbos;

[Table("works")]
public class WorkDbo
{
    [Column("id"), Key]
    public required Guid Id { get; set; }

    [Column("recipientId")]
    public required Guid RecipientId { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("description")]
    public required string Description { get; set; }

    [Column("status")]
    public required WorkStatus WorkStatus { get; set; }

    [Column("createdUtc")]
    public required DateTime CreatedUtc { get; set; }

    [Column("deadLineUtc")]
    public DateTime? DeadLineUtc { get; set; }

    [Column("reminderIntervals")]
    public TimeSpan[]? ReminderIntervals { get; set; }
}