using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository.Converters;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Manager.AuthenticationService.Server.Layers.Repository;

public class AuthorizationModelRepository(
    IAuthorizationModelConverter authorizationModelConverter,
    IDataContext dataContext
) : IAuthorizationModelRepository
{
    public Task CreateAsync(AuthorizationModelWithApiKeyHashDto authorizationModelWithApiKeyHashDto)
    {
        var authorizationModelWithApiKeyHashDbo =
            authorizationModelConverter.ToDbo(authorizationModelWithApiKeyHashDto);
        return dataContext.InsertAsync(authorizationModelWithApiKeyHashDbo);
    }

    public Task UpdateAsync(AuthorizationModelDto authorizationModelDto)
    {
        var authorizationModelDbo = authorizationModelConverter.ToDbo(authorizationModelDto);
        return dataContext.UpdateAsync(authorizationModelDbo);
    }

    public async Task<AuthorizationModelWithApiKeyHashDto?> FindAsync(Guid authorizationModelId)
    {
        var authorizationModelWithApiKeyHashDbo =
            await dataContext.FindAsync<AuthorizationModelWithApiKeyHashDbo, Guid>(authorizationModelId);
        return authorizationModelWithApiKeyHashDbo is null
            ? null
            : authorizationModelConverter.ToDto(authorizationModelWithApiKeyHashDbo);
    }

    public async Task<AuthorizationModelWithApiKeyHashDto> ReadAsync(Guid authorizationModelId)
    {
        var authorizationModelWithApiKeyHashDbo = await dataContext
            .ExecuteReadAsync<AuthorizationModelWithApiKeyHashDbo, AuthorizationModelWithApiKeyHashDbo>(q
                => q.Where(x => x.Id == authorizationModelId)
                    .SingleAsync()
            );
        return authorizationModelConverter.ToDto(authorizationModelWithApiKeyHashDbo);
    }

    public async Task<AuthorizationModelWithApiKeyHashDto?> FindAsync(
        string owner,
        string[] services,
        string[] resources
    )
    {
        var authorizationModelWithApiKeyHashDbo = await dataContext
            .ExecuteReadAsync<AuthorizationModelWithApiKeyHashDbo, AuthorizationModelWithApiKeyHashDbo?>(q
                => q.Where(x => x.ApiKeyOwner == owner)
                    .Where(x => x.AvailableServices.SequenceEqual(services))
                    .Where(x => x.AvailableResources.SequenceEqual(resources))
                    .FirstOrDefaultAsync()
            );
        return authorizationModelWithApiKeyHashDbo is null
            ? null
            : authorizationModelConverter.ToDto(authorizationModelWithApiKeyHashDbo);
    }

    public Task DeleteAsync(AuthorizationModelDto authorizationModelDto)
    {
        var authorizationModelDbo = authorizationModelConverter.ToDbo(authorizationModelDto);
        return dataContext.DeleteAsync(authorizationModelDbo);
    }

    public async Task<AuthorizationModelDto[]> SelectByExpirationTicksAsync(
        long inclusiveMaxExpirationTicks
    )
    {
        var authorizationModelDbos =
            await dataContext.ExecuteReadAsync<AuthorizationModelDbo, AuthorizationModelDbo[]>(q
                => q.Where(x => x.ExpirationUtcTicks.HasValue)
                    .Where(x => x.ExpirationUtcTicks!.Value <= inclusiveMaxExpirationTicks)
                    .Where(x => x.State == AuthorizationModelState.Active)
                    .ToArrayAsync()
            );

        return authorizationModelDbos.Select(authorizationModelConverter.ToDto).ToArray();
    }

    public Task SetStatusAsync(AuthorizationModelState state, params Guid[] authorizationModelIds)
    {
        return dataContext.UpdatePropertiesAsync<AuthorizationModelDbo, Guid>(
            x => x.SetProperty(a => a.State, state),
            x => x.Id,
            authorizationModelIds
        );
    }
}