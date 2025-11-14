using System;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Api.Responses;

public record RecipientAuthorizationResponse(
    Guid RecipientId,
    string RequestedService,
    string RequestedResource,
    RecipientAuthorizationStatus RecipientAuthorizationStatus
);