using System;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record CreateAuthorizationModelDto(
    string Owner,
    string[] AvailableServices,
    string[] AvailableResources,
    DateTime? ExpirationDateUtc
);