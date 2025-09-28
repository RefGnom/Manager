using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Manager.AuthenticationService.Server.Layers.Repository;

public interface IAuthorizationModelRepository
{
    Task CreateAsync(AuthorizationModelWithApiKeyHashDbo authorizationModelWithApiKeyHashDbo);
    Task UpdateAsync(AuthorizationModelDbo authorizationModelDbo);
    Task<AuthorizationModelWithApiKeyHashDbo?> FindAsync(Guid authorizationModelId);
    Task<AuthorizationModelWithApiKeyHashDbo> ReadAsync(Guid authorizationModelId);
    Task<AuthorizationModelWithApiKeyHashDbo?> FindAsync(string owner, string[] services, string[] resources);
    Task DeleteAsync(AuthorizationModelDbo authorizationModelDbo);
    Task<AuthorizationModelDbo[]> SelectByExpirationTicksAsync(long exclusiveMaxExpirationTicks);
    Task SetStatusAsync(AuthorizationModelState state, params Guid[] authorizationModelIds);
}

public class AuthorizationModelRepository(
    IDataContext dataContext
) : IAuthorizationModelRepository
{
    public Task CreateAsync(AuthorizationModelWithApiKeyHashDbo authorizationModelWithApiKeyHashDbo)
    {
        return dataContext.InsertAsync(authorizationModelWithApiKeyHashDbo);
    }

    public Task UpdateAsync(AuthorizationModelDbo authorizationModelDbo)
    {
        return dataContext.UpdateAsync(authorizationModelDbo);
    }

    public Task<AuthorizationModelWithApiKeyHashDbo?> FindAsync(Guid authorizationModelId)
    {
        return dataContext.FindAsync<AuthorizationModelWithApiKeyHashDbo, Guid>(authorizationModelId);
    }

    public Task<AuthorizationModelWithApiKeyHashDbo> ReadAsync(Guid authorizationModelId)
    {
        return dataContext.ExecuteReadAsync<AuthorizationModelWithApiKeyHashDbo, AuthorizationModelWithApiKeyHashDbo>(q
            => q.Where(x => x.Id == authorizationModelId)
                .SingleAsync()
        );
    }

    public Task<AuthorizationModelWithApiKeyHashDbo?> FindAsync(string owner, string[] services, string[] resources)
    {
        return dataContext.ExecuteReadAsync<AuthorizationModelWithApiKeyHashDbo, AuthorizationModelWithApiKeyHashDbo?>(q
            => q.Where(x => x.ApiKeyOwner == owner)
                .Where(x => x.AvailableServices.SequenceEqual(services))
                .Where(x => x.AvailableResources.SequenceEqual(resources))
                .FirstOrDefaultAsync()
        );
    }

    public Task DeleteAsync(AuthorizationModelDbo authorizationModelDbo)
    {
        return dataContext.DeleteAsync(authorizationModelDbo);
    }

    public Task<AuthorizationModelDbo[]> SelectByExpirationTicksAsync(long inclusiveMaxExpirationTicks)
    {
        return dataContext.ExecuteReadAsync<AuthorizationModelDbo, AuthorizationModelDbo[]>(q
            => q.Where(x => x.ExpirationUtcTicks.HasValue)
                .Where(x => x.ExpirationUtcTicks!.Value <= inclusiveMaxExpirationTicks)
                .Where(x => x.State == AuthorizationModelState.Active)
                .ToArrayAsync()
        );
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