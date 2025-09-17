using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Converters;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;
using Manager.Core.Common.HelperObjects.Result;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthorizationModelService
{
    Task<Result<CreateAuthorizationModelErrorCode>> CreateAsync(AuthorizationModelDto authorizationModelDto);
}

public class AuthorizationModelService(
    IAuthorizationModelRepository authorizationModelRepository,
    IAuthorizationModelConverter authorizationModelConverter
) : IAuthorizationModelService
{
    public async Task<Result<CreateAuthorizationModelErrorCode>> CreateAsync(
        AuthorizationModelDto authorizationModelDto
    )
    {
        var existAuthorizationModel = await authorizationModelRepository.FindAsync(authorizationModelDto.ApiKeyHash);
        if (existAuthorizationModel != null)
        {
            return CreateAuthorizationModelErrorCode.AuthorizationModelAlreadyExists;
        }

        var authorizationModelDbo = authorizationModelConverter.ToDbo(authorizationModelDto);
        await authorizationModelRepository.CreateAsync(authorizationModelDbo);
        return Result<CreateAuthorizationModelErrorCode>.Ok();
    }
}