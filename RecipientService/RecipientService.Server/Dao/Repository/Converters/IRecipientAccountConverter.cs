using System;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Converters;

public interface IRecipientAccountConverter
{
    RecipientAccountDbo ToDbo(RecipientAccount recipientAccount, Guid stateId);

    RecipientAccount ToDto(RecipientAccountDbo recipientAccountDbo, RecipientAccountState recipientAccountState);
}