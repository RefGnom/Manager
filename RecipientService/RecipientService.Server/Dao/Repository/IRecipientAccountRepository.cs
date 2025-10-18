using System;
using System.Threading.Tasks;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository;

public interface IRecipientAccountRepository
{
    Task CreateAsync(RecipientAccount recipientAccount);
    Task<RecipientAccount?> FindAsync(Guid recipientAccountId);
}