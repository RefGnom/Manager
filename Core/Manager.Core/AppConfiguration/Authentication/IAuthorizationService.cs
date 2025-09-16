using System.Threading.Tasks;

namespace Manager.Core.AppConfiguration.Authentication;

public interface IAuthorizationService
{
    Task<AuthorizationModel?> FindAuthorizationModelAsync(string apiKey, string resource);
}

public class AuthorizationService : IAuthorizationService
{
    public Task<AuthorizationModel?> FindAuthorizationModelAsync(string apiKey, string resource)
    {
        return Task.FromResult<AuthorizationModel?>(new AuthorizationModel { HasAccess = true });
    }
}

public class AuthorizationModel
{
    public bool HasAccess { get; set; }
}