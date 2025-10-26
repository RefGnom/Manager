using System;
using System.Threading.Tasks;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository;

public interface IRecipientAccountRepository
{
    Task CreateAsync(RecipientAccountWithPasswordHash recipientAccount);
    Task<RecipientAccountWithPasswordHash?> FindAsync(Guid recipientAccountId);
    Task<RecipientAccountWithPasswordHash?> FindByLoginAsync(string login);
}