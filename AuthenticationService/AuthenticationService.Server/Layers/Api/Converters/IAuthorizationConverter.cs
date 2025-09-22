using System;
using Manager.AuthenticationService.Server.Layers.Api.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

namespace Manager.AuthenticationService.Server.Layers.Api.Converters;

public interface IAuthorizationConverter
{
    CreateAuthorizationModelDto ToDto(CreateAuthorizationModelRequest createAuthorizationModelRequest);

    AuthorizationModelDto Map(
        AuthorizationModelDto existAuthorizationModelDto,
        PatchAuthorizationModelRequest createAuthorizationModelRequest
    );
}

public class AuthorizationConverter : IAuthorizationConverter
{
    public CreateAuthorizationModelDto ToDto(CreateAuthorizationModelRequest createAuthorizationModelRequest)
    {
        return new CreateAuthorizationModelDto(
            createAuthorizationModelRequest.Owner,
            createAuthorizationModelRequest.AvailableServices,
            createAuthorizationModelRequest.AvailableResources,
            DateTime.UtcNow.Add(TimeSpan.FromDays(createAuthorizationModelRequest.DaysAlive ?? 0))
        );
    }

    public AuthorizationModelDto Map(
        AuthorizationModelDto existAuthorizationModelDto,
        PatchAuthorizationModelRequest createAuthorizationModelRequest
    )
    {
        return new AuthorizationModelDto(
            existAuthorizationModelDto.Id,
            createAuthorizationModelRequest.Owner ?? existAuthorizationModelDto.Owner,
            createAuthorizationModelRequest.AvailableServices ?? existAuthorizationModelDto.AvailableServices,
            createAuthorizationModelRequest.AvailableResources ?? existAuthorizationModelDto.AvailableResources,
            false,
            existAuthorizationModelDto.CreatedUtcTicks,
            createAuthorizationModelRequest.DaysAlive.HasValue
                ? DateTime.UtcNow.Add(TimeSpan.FromDays(createAuthorizationModelRequest.DaysAlive.Value)).Ticks
                : existAuthorizationModelDto.ExpirationUtcTicks
        );
    }
}