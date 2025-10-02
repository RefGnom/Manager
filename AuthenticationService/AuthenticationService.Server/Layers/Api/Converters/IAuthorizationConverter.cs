using System;
using Manager.AuthenticationService.Server.Layers.Api.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Time;

namespace Manager.AuthenticationService.Server.Layers.Api.Converters;

public interface IAuthorizationConverter
{
    CreateAuthorizationModelDto ToDto(CreateAuthorizationModelRequest createAuthorizationModelRequest);

    AuthorizationModelDto Map(
        AuthorizationModelDto existAuthorizationModelDto,
        PatchAuthorizationModelRequest createAuthorizationModelRequest
    );
}

public class AuthorizationConverter(
    IDateTimeProvider dateTimeProvider
) : IAuthorizationConverter
{
    public CreateAuthorizationModelDto ToDto(CreateAuthorizationModelRequest createAuthorizationModelRequest) => new(
        createAuthorizationModelRequest.Owner,
        createAuthorizationModelRequest.AvailableServices,
        createAuthorizationModelRequest.AvailableResources,
        createAuthorizationModelRequest.DaysAlive.HasValue
            ? dateTimeProvider.UtcNow.Add(TimeSpan.FromDays(createAuthorizationModelRequest.DaysAlive.Value))
            : null
    );

    public AuthorizationModelDto Map(
        AuthorizationModelDto existAuthorizationModelDto,
        PatchAuthorizationModelRequest createAuthorizationModelRequest
    ) => new(
        existAuthorizationModelDto.Id,
        createAuthorizationModelRequest.Owner ?? existAuthorizationModelDto.Owner,
        createAuthorizationModelRequest.AvailableServices ?? existAuthorizationModelDto.AvailableServices,
        createAuthorizationModelRequest.AvailableResources ?? existAuthorizationModelDto.AvailableResources,
        AuthorizationModelState.Active,
        existAuthorizationModelDto.CreatedUtcTicks,
        createAuthorizationModelRequest.DaysAlive.HasValue
            ? dateTimeProvider.UtcNow.Add(TimeSpan.FromDays(createAuthorizationModelRequest.DaysAlive.Value)).Ticks
            : existAuthorizationModelDto.ExpirationUtcTicks
    );
}