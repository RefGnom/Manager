using System;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record AuthorizationModelDto(
    Guid Id,
    string Owner,
    string[] AvailableServices,
    string[] AvailableResources,
    AuthorizationModelState State,
    long CreatedUtcTicks,
    long? ExpirationUtcTicks
);