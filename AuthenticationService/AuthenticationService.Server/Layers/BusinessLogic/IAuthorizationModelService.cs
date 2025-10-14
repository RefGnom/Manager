using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.HelperObjects.Result;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthorizationModelService
{
    Task<Result<AuthorizationModelWithApiKeyDto, CreateAuthorizationModelErrorCode>> CreateAsync(
        CreateAuthorizationModelDto createAuthorizationModelDto
    );

    Task UpdateAsync(AuthorizationModelDto createAuthorizationModelDto);
    Task<AuthorizationModelDto?> FindAsync(Guid authorizationModelId);
    Task DeleteAsync(AuthorizationModelDto authorizationModelDto);
    Task<AuthorizationModelDto[]> SelectExpiredAsync();
    Task ExpireAsync(params Guid[] authorizationModelIds);
    Task RevokeAsync(params Guid[] authorizationModelIds);
    Task<AuthorizationModelWithApiKeyDto> RecreateAsync(Guid oldAuthorizationModelId, int? daysAlive);
}