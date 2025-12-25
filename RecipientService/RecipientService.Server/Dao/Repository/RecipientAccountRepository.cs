using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Common.Time;
using Manager.Core.EFCore;
using Manager.RecipientService.Server.Dao.Repository.Converters;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;
using Microsoft.EntityFrameworkCore;

namespace Manager.RecipientService.Server.Dao.Repository;

public class RecipientAccountRepository(
    IRecipientAccountStateRepository recipientAccountStateRepository,
    IDataContext<RecipientAccountDbo> dataContext,
    IDateTimeProvider dateTimeProvider,
    IRecipientAccountConverter recipientAccountConverter
) : IRecipientAccountRepository
{
    public async Task CreateAsync(RecipientAccountWithPasswordHash recipientAccount)
    {
        var storedAccountState = await recipientAccountStateRepository.FindOrCreateAsync(recipientAccount.State);

        var recipientAccountDbo = recipientAccountConverter.ToDbo(recipientAccount, storedAccountState.Id);
        recipientAccountDbo.UpdatedAtUtc = dateTimeProvider.UtcNow;
        await dataContext.InsertAsync(recipientAccountDbo);
    }

    public async Task<RecipientAccountWithPasswordHash?> FindAsync(Guid recipientAccountId)
    {
        var recipientAccountDbo = await dataContext.FindAsync(recipientAccountId);
        return await InnerFindAsync(recipientAccountDbo);
    }

    public async Task<RecipientAccountWithPasswordHash?> FindByLoginAsync(string login)
    {
        var recipientAccountDbo = await dataContext.ExecuteReadAsync<RecipientAccountDbo?>(query =>
            query.Where(x => x.Login == login).FirstOrDefaultAsync()
        );
        return await InnerFindAsync(recipientAccountDbo);
    }

    public async Task UpdateAsync(RecipientAccountWithPasswordHash recipientAccount)
    {
        var storedAccountState = await recipientAccountStateRepository.FindOrCreateAsync(recipientAccount.State);

        var recipientAccountDbo = recipientAccountConverter.ToDbo(recipientAccount, storedAccountState.Id);
        recipientAccountDbo.UpdatedAtUtc = dateTimeProvider.UtcNow;
        await dataContext.UpdateAsync(recipientAccountDbo);
    }

    public async Task UpdateAsync(RecipientAccount recipientAccount)
    {
        var foundAccount = await dataContext.FindAsync(recipientAccount.Id);
        if (foundAccount == null)
        {
            throw new EntityNotFoundException($"Не нашли аккаунт {recipientAccount.Id} при обновлении");
        }

        var storedAccountState = await recipientAccountStateRepository.FindOrCreateAsync(recipientAccount.State);
        var recipientAccountDbo = recipientAccountConverter.ToDbo(
            foundAccount,
            recipientAccount,
            storedAccountState.Id
        );
        recipientAccountDbo.UpdatedAtUtc = dateTimeProvider.UtcNow;
        await dataContext.UpdateAsync(recipientAccountDbo);
    }

    private async Task<RecipientAccountWithPasswordHash?> InnerFindAsync(RecipientAccountDbo? recipientAccountDbo)
    {
        if (recipientAccountDbo == null)
        {
            return null;
        }

        var recipientAccountState = await recipientAccountStateRepository.ReadAsync(recipientAccountDbo.AccountStateId);
        return recipientAccountConverter.ToDto(recipientAccountDbo, recipientAccountState);
    }
}