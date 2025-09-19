using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Converters;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;
using Manager.Core.Common.HelperObjects.Result;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthorizationModelService
{
    Task<Result<AuthorizationModelDto, CreateAuthorizationModelErrorCode>> CreateAsync(
        CreateAuthorizationModelDto createAuthorizationModelDto
    );
}

public class AuthorizationModelService(
    IAuthorizationModelRepository authorizationModelRepository,
    IAuthorizationModelConverter authorizationModelConverter,
    IAuthorizationModelFactory authorizationModelFactory
) : IAuthorizationModelService
{
    public async Task<Result<AuthorizationModelDto, CreateAuthorizationModelErrorCode>> CreateAsync(
        CreateAuthorizationModelDto createAuthorizationModelDto
    )
    {
        var authorizationModelDto = authorizationModelFactory.Create(createAuthorizationModelDto);
        var authorizationModelDbo = authorizationModelConverter.ToDbo(authorizationModelDto);

        await authorizationModelRepository.CreateAsync(authorizationModelDbo);
        return authorizationModelDto;
    }
}