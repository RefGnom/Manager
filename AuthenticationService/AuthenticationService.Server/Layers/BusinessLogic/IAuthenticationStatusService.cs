using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthenticationStatusService
{
    Task<AuthenticationStatusDto> GetAsync(string apiKeyHash, string service, string resource);
}

public class AuthenticationStatusService(
    IAuthorizationModelRepository authorizationModelRepository
) : IAuthenticationStatusService
{
    public async Task<AuthenticationStatusDto> GetAsync(string apiKeyHash, string service, string resource)
    {
        var authorizationModelDbo = await authorizationModelRepository.FindAsync(apiKeyHash);
        if (authorizationModelDbo == null)
        {
            return AuthenticationCode.ApiKeyNotFound;
        }

        if (authorizationModelDbo.AvailableResources.Contains(resource) &&
            authorizationModelDbo.AvailableServices.Contains(service))
        {
            return AuthenticationCode.Authenticated;
        }

        return AuthenticationCode.ResourceNotAvailable;
    }
}