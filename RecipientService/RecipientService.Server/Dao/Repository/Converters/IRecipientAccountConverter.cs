using System;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Converters;

public interface IRecipientAccountConverter
{
    RecipientAccountDbo ToDbo(RecipientAccountWithPasswordHash recipientAccount, Guid stateId);
    RecipientAccountDbo ToDbo(RecipientAccountDbo original, RecipientAccount updated, Guid stateId);

    RecipientAccountWithPasswordHash ToDto(
        RecipientAccountDbo recipientAccountDbo,
        RecipientAccountState recipientAccountState
    );
}