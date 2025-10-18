using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Converters;

public class RecipientAccountStateConverter : IRecipientAccountStateConverter
{
    public RecipientAccountStateWithId ToDto(RecipientAccountStateDbo recipientAccountState) => new(
        recipientAccountState.Id,
        recipientAccountState.AccountState,
        recipientAccountState.StateReason
    );
}