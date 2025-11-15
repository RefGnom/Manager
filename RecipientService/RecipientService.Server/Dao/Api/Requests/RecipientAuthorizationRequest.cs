using System;

namespace Manager.RecipientService.Server.Dao.Api.Requests;

public record RecipientAuthorizationRequest(
    Guid RecipientId,
    string RequestedService,
    string RequestedResource
);