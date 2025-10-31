using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.EFCore;
using Manager.RecipientService.Server.Dao.Repository.Converters;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Dao.Repository.Factories;
using Manager.RecipientService.Server.Implementation.Domain;
using Microsoft.EntityFrameworkCore;

namespace Manager.RecipientService.Server.Dao.Repository;

public class RecipientAccountStateRepository(
    IDataContext<RecipientAccountStateDbo> dataContext,
    IRecipientAccountStateConverter recipientAccountStateConverter,
    IRecipientAccountStateDboFactory recipientAccountStateDboFactory
) : IRecipientAccountStateRepository
{
    public async Task<RecipientAccountStateWithId> ReadAsync(Guid stateId)
    {
        var recipientAccountStateDbo = await dataContext.ReadAsync(stateId);
        return recipientAccountStateConverter.ToDto(recipientAccountStateDbo);
    }

    public async Task<RecipientAccountStateWithId> FindOrCreateAsync(RecipientAccountState recipientAccountState)
    {
        var recipientAccountStateDbo = await dataContext.ExecuteReadAsync<RecipientAccountStateDbo?>(query =>
            query.Where(x => x.AccountState == recipientAccountState.AccountState)
                .Where(x => x.StateReason == recipientAccountState.StateReason)
                .FirstOrDefaultAsync()
        );

        if (recipientAccountStateDbo is not null)
        {
            return recipientAccountStateConverter.ToDto(recipientAccountStateDbo);
        }

        var createdAccountState = recipientAccountStateDboFactory.Create(recipientAccountState);
        await dataContext.InsertAsync(createdAccountState);
        return recipientAccountStateConverter.ToDto(createdAccountState);
    }
}