using System;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Converters;

public class RecipientAccountConverter : IRecipientAccountConverter
{
    public RecipientAccountDbo ToDbo(RecipientAccount recipientAccount, Guid stateId) => new()
    {
        Id = recipientAccount.Id,
        Login = recipientAccount.Login,
        PasswordHash = recipientAccount.PasswordHash,
        AccountStateId = stateId,
        TimeZoneInfo = recipientAccount.TimeZoneInfo,
        CreatedAtUtc = recipientAccount.CreatedAtUtc,
        UpdatedAtUtc = recipientAccount.UpdatedAtUtc,
    };

    public RecipientAccount ToDto(
        RecipientAccountDbo recipientAccountDbo,
        RecipientAccountState recipientAccountState
    ) => new(
        recipientAccountDbo.Id,
        recipientAccountDbo.Login,
        recipientAccountDbo.PasswordHash,
        recipientAccountState,
        recipientAccountDbo.TimeZoneInfo,
        recipientAccountDbo.CreatedAtUtc,
        recipientAccountDbo.UpdatedAtUtc
    );
}