using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

namespace Manager.AuthenticationService.Server.Layers.Repository;

public interface IAuthorizationModelRepository
{
    Task CreateAsync(AuthorizationModelWithApiKeyHashDto authorizationModelWithApiKeyHashDto);
    Task UpdateAsync(AuthorizationModelDto authorizationModelDto);
    Task<AuthorizationModelWithApiKeyHashDto?> FindAsync(Guid authorizationModelId);
    Task<AuthorizationModelWithApiKeyHashDto> ReadAsync(Guid authorizationModelId);
    Task<AuthorizationModelWithApiKeyHashDto?> FindAsync(string owner, string[] services, string[] resources);
    Task DeleteAsync(AuthorizationModelDto authorizationModelDto);
    Task<AuthorizationModelDto[]> SelectByExpirationTicksAsync(long inclusiveMaxExpirationTicks);
    Task SetStatusAsync(AuthorizationModelState state, params Guid[] authorizationModelIds);
}