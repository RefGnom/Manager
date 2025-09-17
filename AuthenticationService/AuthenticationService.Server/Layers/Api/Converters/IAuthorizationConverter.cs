using System;
using Manager.AuthenticationService.Server.Layers.Api.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Time;

namespace Manager.AuthenticationService.Server.Layers.Api.Converters;

public interface IAuthorizationConverter
{
    AuthorizationModelDto ToDto(AuthorizationModelRequest authorizationModelRequest, string apiKeyHash);
}

public class AuthorizationConverter(
    IDateTimeProvider dateTimeProvider
) : IAuthorizationConverter
{
    public AuthorizationModelDto ToDto(AuthorizationModelRequest authorizationModelRequest, string apiKeyHash)
    {
        return new AuthorizationModelDto(
            Guid.NewGuid(),
            apiKeyHash,
            authorizationModelRequest.AvailableServices,
            authorizationModelRequest.AvailableResources,
            dateTimeProvider.UtcTicks,
            authorizationModelRequest.ExpirationDateUtc?.Ticks
        );
    }
}