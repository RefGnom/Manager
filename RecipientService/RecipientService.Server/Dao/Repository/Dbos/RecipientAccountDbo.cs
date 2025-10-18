using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manager.RecipientService.Server.Dao.Repository.Dbos;

[Table("recipient_account")]
public class RecipientAccountDbo
{
    [Key, Column("id")]
    public required Guid Id { get; set; }

    [Column("login")]
    public required string Login { get; set; }

    [Column("password_hash")]
    public required string PasswordHash { get; set; }

    [Column("account_state_id")]
    public required Guid AccountStateId { get; set; }

    [Column("timezone_info")]
    public required TimeZoneInfo TimeZoneInfo { get; set; }

    [Column("created_at_utc")]
    public required DateTime CreatedAtUtc { get; set; }

    [Column("updated_at_utc")]
    public required DateTime UpdatedAtUtc { get; set; }
}