using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Converters;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;
using Manager.Core.Common.HelperObjects.Result;
using Microsoft.Extensions.Logging;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IAuthorizationModelService
{
    Task<Result<AuthorizationModelWithApiKeyDto, CreateAuthorizationModelErrorCode>> CreateAsync(
        CreateAuthorizationModelDto createAuthorizationModelDto
    );

    Task UpdateAsync(AuthorizationModelDto createAuthorizationModelDto);
    Task<AuthorizationModelDto?> FindAsync(Guid authorizationModelId);
}

public class AuthorizationModelService(
    IAuthorizationModelRepository authorizationModelRepository,
    IAuthorizationModelConverter authorizationModelConverter,
    IAuthorizationModelFactory authorizationModelFactory,
    ILogger<AuthorizationModelService> logger
) : IAuthorizationModelService
{
    public async Task<Result<AuthorizationModelWithApiKeyDto, CreateAuthorizationModelErrorCode>> CreateAsync(
        CreateAuthorizationModelDto createAuthorizationModelDto
    )
    {
        logger.LogInformation("Создаём модель авторизации для пользователя {owner}", createAuthorizationModelDto.Owner);
        var foundAuthModel = await authorizationModelRepository.FindAsync(
            createAuthorizationModelDto.Owner,
            createAuthorizationModelDto.AvailableServices,
            createAuthorizationModelDto.AvailableResources
        );
        if (foundAuthModel is not null)
        {
            return CreateAuthorizationModelErrorCode.AuthorizationModelAlreadyExists;
        }

        var authorizationModelDtoWithApiKey = authorizationModelFactory.Create(createAuthorizationModelDto);
        var authorizationModelDbo = authorizationModelConverter.ToDbo(authorizationModelDtoWithApiKey);

        await authorizationModelRepository.CreateAsync(authorizationModelDbo);
        return authorizationModelDtoWithApiKey;
    }

    public Task UpdateAsync(AuthorizationModelDto createAuthorizationModelDto)
    {
        var authorizationModelDbo = authorizationModelConverter.ToDbo(createAuthorizationModelDto);
        return authorizationModelRepository.UpdateAsync(authorizationModelDbo);
    }

    public async Task<AuthorizationModelDto?> FindAsync(Guid authorizationModelId)
    {
        var authorizationModelDbo = await authorizationModelRepository.FindAsync(authorizationModelId);
        return authorizationModelDbo is null ? null : authorizationModelConverter.ToDto(authorizationModelDbo);
    }
}