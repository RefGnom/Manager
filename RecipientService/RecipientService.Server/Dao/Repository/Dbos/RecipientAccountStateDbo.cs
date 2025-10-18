using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Dbos;

[Table("recipient_account_state")]
public class RecipientAccountStateDbo
{
    [Key, Column("id")]
    public required Guid Id { get; set; }

    [Column("state")]
    public required AccountState AccountState { get; set; }

    [Column("state_reason")]
    public required StateReason StateReason { get; set; }
}