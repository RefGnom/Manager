using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Factories;

public interface IRecipientAccountStateDboFactory
{
    RecipientAccountStateDbo Create(RecipientAccountState recipientAccountState);
}