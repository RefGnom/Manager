using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.AppConfiguration.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Manager.AuthenticationService.Server.Layers.Repository;

public interface IAuthorizationModelRepository
{
    Task CreateAsync(AuthorizationModelWithApiKeyHashDbo authorizationModelWithApiKeyHashDbo);
    Task UpdateAsync(AuthorizationModelDbo authorizationModelDbo);
    Task<AuthorizationModelWithApiKeyHashDbo?> FindAsync(Guid authorizationModelId);
    Task<AuthorizationModelWithApiKeyHashDbo?> FindAsync(string owner, string[] services, string[] resources);
    Task DeleteAsync(AuthorizationModelDbo authorizationModelDbo);
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
}