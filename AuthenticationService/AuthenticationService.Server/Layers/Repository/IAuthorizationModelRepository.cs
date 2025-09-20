using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.AppConfiguration.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Manager.AuthenticationService.Server.Layers.Repository;

public interface IAuthorizationModelRepository
{
    Task CreateAsync(AuthorizationModelDbo authorizationModelDbo);
    Task<AuthorizationModelDbo?> FindAsync(Guid authorizationModelId);
    Task<AuthorizationModelDbo?> FindAsync(string owner, string[] services, string[] resources);
}

public class AuthorizationModelRepository(
    IDataContext dataContext
) : IAuthorizationModelRepository
{
    public Task CreateAsync(AuthorizationModelDbo authorizationModelDbo)
    {
        return dataContext.InsertAsync(authorizationModelDbo);
    }

    public Task<AuthorizationModelDbo?> FindAsync(Guid authorizationModelId)
    {
        return dataContext.FindAsync<AuthorizationModelDbo, Guid>(authorizationModelId);
    }

    public Task<AuthorizationModelDbo?> FindAsync(string owner, string[] services, string[] resources)
    {
        return dataContext.ExecuteReadAsync<AuthorizationModelDbo, AuthorizationModelDbo?>(q => q
            .Where(x => x.ApiKeyOwner == owner)
            .Where(x => x.AvailableServices.SequenceEqual(services))
            .Where(x => x.AvailableResources.SequenceEqual(resources))
            .FirstOrDefaultAsync()
        );
    }
}