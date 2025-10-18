using System;
using System.Threading.Tasks;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository;

public interface IRecipientAccountStateRepository
{
    Task<RecipientAccountStateWithId> ReadAsync(Guid id);
    Task<RecipientAccountStateWithId> FindOrCreateAsync(RecipientAccountState recipientAccountState);
}