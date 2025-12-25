using System;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Converters;

public class RecipientAccountConverter : IRecipientAccountConverter
{
    public RecipientAccountDbo ToDbo(RecipientAccountWithPasswordHash recipientAccount, Guid stateId) => new()
    {
        Id = recipientAccount.Id,
        Login = recipientAccount.Login,
        PasswordHash = recipientAccount.PasswordHash,
        AccountStateId = stateId,
        TimeZoneInfoId = recipientAccount.TimeZoneInfo.Id,
        CreatedAtUtc = recipientAccount.CreatedAtUtc,
        UpdatedAtUtc = recipientAccount.UpdatedAtUtc,
    };

    public RecipientAccountDbo ToDbo(RecipientAccountDbo original, RecipientAccount updated, Guid stateId) => new()
    {
        Id = original.Id,
        Login = updated.Login,
        PasswordHash = original.PasswordHash,
        AccountStateId = stateId,
        TimeZoneInfoId = updated.TimeZoneInfo.Id,
        CreatedAtUtc = original.CreatedAtUtc,
        UpdatedAtUtc = updated.UpdatedAtUtc,
    };

    public RecipientAccountWithPasswordHash ToDto(
        RecipientAccountDbo recipientAccountDbo,
        RecipientAccountState recipientAccountState
    ) => new(
        recipientAccountDbo.Id,
        recipientAccountDbo.Login,
        recipientAccountDbo.PasswordHash,
        recipientAccountState,
        TimeZoneInfo.FindSystemTimeZoneById(recipientAccountDbo.TimeZoneInfoId),
        recipientAccountDbo.CreatedAtUtc,
        recipientAccountDbo.UpdatedAtUtc
    );
}