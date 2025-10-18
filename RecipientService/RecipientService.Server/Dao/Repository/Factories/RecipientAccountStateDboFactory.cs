using Manager.Core.Common.Factories;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Factories;

public class RecipientAccountStateDboFactory(
    IGuidFactory guidFactory
) : IRecipientAccountStateDboFactory
{
    public RecipientAccountStateDbo Create(RecipientAccountState recipientAccountState) => new()
    {
        Id = guidFactory.Create(),
        AccountState = recipientAccountState.AccountState,
        StateReason = recipientAccountState.StateReason,
    };
}