using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.AppConfiguration.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Manager.AuthenticationService.Server.Layers.Repository;

public interface IAuthorizationModelRepository
{
    Task<AuthorizationModelDbo?> FindAsync(string apiKeyHash);
    Task CreateAsync(AuthorizationModelDbo authorizationModelDbo);
}

public class AuthorizationModelRepository(
    IDataContext dataContext
) : IAuthorizationModelRepository
{
    public Task<AuthorizationModelDbo?> FindAsync(string apiKeyHash)
    {
        return dataContext.ExecuteReadAsync<AuthorizationModelDbo, AuthorizationModelDbo?>(q => q
            .Where(x => x.ApiKeyHash == apiKeyHash)
            .FirstOrDefaultAsync()
        );
    }

    public Task CreateAsync(AuthorizationModelDbo authorizationModelDbo)
    {
        return dataContext.InsertAsync(authorizationModelDbo);
    }
}