using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.AppConfiguration.DataBase;

namespace Manager.AuthenticationService.Server.Layers.Repository;

public interface IAuthorizationModelRepository
{
    Task<AuthorizationModelDbo?> FindAsync(Guid authorizationModelId);
    Task CreateAsync(AuthorizationModelDbo authorizationModelDbo);
}

public class AuthorizationModelRepository(
    IDataContext dataContext
) : IAuthorizationModelRepository
{
    public Task<AuthorizationModelDbo?> FindAsync(Guid authorizationModelId)
    {
        return dataContext.FindAsync<AuthorizationModelDbo, Guid>(authorizationModelId);
    }

    public Task CreateAsync(AuthorizationModelDbo authorizationModelDbo)
    {
        return dataContext.InsertAsync(authorizationModelDbo);
    }
}