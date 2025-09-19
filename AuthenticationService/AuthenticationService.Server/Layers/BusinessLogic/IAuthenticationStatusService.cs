using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthenticationStatusService
{
    Task<AuthenticationStatusDto> GetAsync(string apiKey, string service, string resource);
}

public class AuthenticationStatusService(
    IAuthorizationModelRepository authorizationModelRepository,
    IApiKeyService apiKeyService
) : IAuthenticationStatusService
{
    public async Task<AuthenticationStatusDto> GetAsync(string apiKey, string service, string resource)
    {
        var authorizationModelId = apiKeyService.ExtractAuthorizationModelId(apiKey);
        var authModelDbo = await authorizationModelRepository.FindAsync(authorizationModelId);
        if (authModelDbo == null || !apiKeyService.VerifyHashedApiKey(authModelDbo.ApiKeyHash, apiKey))
        {
            return AuthenticationCode.ApiKeyNotFound;
        }

        if (authModelDbo.AvailableResources.Contains(resource) && authModelDbo.AvailableServices.Contains(service))
        {
            return AuthenticationCode.Authenticated;
        }

        return AuthenticationCode.ResourceNotAvailable;
    }
}