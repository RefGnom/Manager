using System.Linq;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;
using Microsoft.Extensions.Logging;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthenticationStatusService
{
    Task<AuthenticationStatusDto> GetAsync(string apiKey, string service, string resource);
}

public class AuthenticationStatusService(
    IAuthorizationModelRepository authorizationModelRepository,
    IApiKeyService apiKeyService,
    ILogger<AuthenticationStatusService> logger
) : IAuthenticationStatusService
{
    public async Task<AuthenticationStatusDto> GetAsync(string apiKey, string service, string resource)
    {
        logger.LogInformation("Начали получение статуса аутентификации по апи ключу для сервиса и ресурса");
        var extractAuthorizationModelIdResult = apiKeyService.TryExtractAuthorizationModelId(apiKey);
        if (extractAuthorizationModelIdResult.IsFailure)
        {
            return AuthenticationCode.ApiKeyNotFound;
        }

        var authModelDbo = await authorizationModelRepository.FindAsync(extractAuthorizationModelIdResult.Value);
        if (authModelDbo == null || !apiKeyService.VerifyHashedApiKey(authModelDbo.ApiKeyHash, apiKey))
        {
            return AuthenticationCode.ApiKeyNotFound;
        }

        if (!authModelDbo.AvailableResources.Contains(resource) || !authModelDbo.AvailableServices.Contains(service))
        {
            return AuthenticationCode.ResourceNotAvailable;
        }

        return authModelDbo.IsRevoked ? AuthenticationCode.ApiKeyRevoked : AuthenticationCode.Authenticated;
    }
}