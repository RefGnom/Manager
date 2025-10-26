using System;
using System.Threading.Tasks;
using Manager.Core.Common.Time;
using Manager.Core.EFCore;
using Manager.RecipientService.Server.Dao.Repository.Converters;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

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
        if (recipientAccountDbo == null)
        {
            return null;
        }

        var recipientAccountState = await recipientAccountStateRepository.ReadAsync(recipientAccountDbo.AccountStateId);
        return recipientAccountConverter.ToDto(recipientAccountDbo, recipientAccountState);
    }
}