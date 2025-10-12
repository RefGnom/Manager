using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;
using Manager.Core.Common.HelperObjects.Result;
using Manager.Core.Common.Time;
using Microsoft.Extensions.Logging;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public class AuthorizationModelService(
    IAuthorizationModelRepository authorizationModelRepository,
    IAuthorizationModelFactory authorizationModelFactory,
    IDateTimeProvider dateTimeProvider,
    IAuthorizationModelHashService authorizationModelHashService,
    ILogger<AuthorizationModelService> logger
) : IAuthorizationModelService
{
    private const int DaysAliveDefault = 365;

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
        var authorizationModelWithApiKeyHashDto = authorizationModelHashService.Hash(authorizationModelDtoWithApiKey);

        await authorizationModelRepository.CreateAsync(authorizationModelWithApiKeyHashDto);
        return authorizationModelDtoWithApiKey;
    }

    public Task UpdateAsync(AuthorizationModelDto createAuthorizationModelDto)
    {
        return authorizationModelRepository.UpdateAsync(createAuthorizationModelDto);
    }

    public async Task<AuthorizationModelDto?> FindAsync(Guid authorizationModelId)
    {
        return await authorizationModelRepository.FindAsync(authorizationModelId);
    }

    public Task DeleteAsync(AuthorizationModelDto authorizationModelDto)
    {
        return authorizationModelRepository.DeleteAsync(authorizationModelDto);
    }

    public Task<AuthorizationModelDto[]> SelectExpiredAsync()
    {
        var currentTicks = dateTimeProvider.UtcTicks;
        return authorizationModelRepository.SelectByExpirationTicksAsync(currentTicks);
    }

    public Task ExpireAsync(params Guid[] authorizationModelIds) =>
        authorizationModelRepository.SetStatusAsync(AuthorizationModelState.Expired, authorizationModelIds);

    public Task RevokeAsync(params Guid[] authorizationModelIds) =>
        authorizationModelRepository.SetStatusAsync(AuthorizationModelState.Revoked, authorizationModelIds);

    public async Task<AuthorizationModelWithApiKeyDto> RecreateAsync(Guid oldAuthorizationModelId, int? daysAlive)
    {
        var oldAuthorizationModelDbo = await authorizationModelRepository.ReadAsync(oldAuthorizationModelId);
        var authorizationModelDtoWithApiKey = authorizationModelFactory.Create(
            oldAuthorizationModelDbo.Owner,
            oldAuthorizationModelDbo.AvailableServices,
            oldAuthorizationModelDbo.AvailableResources,
            dateTimeProvider.UtcNow.Add(TimeSpan.FromDays(daysAlive ?? DaysAliveDefault)).Ticks
        );
        var authorizationModelWithApiKeyHashDto = authorizationModelHashService.Hash(authorizationModelDtoWithApiKey);

        await authorizationModelRepository.CreateAsync(authorizationModelWithApiKeyHashDto);
        return authorizationModelDtoWithApiKey;
    }
}