using System;

namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public record PatchRecipientAccountRequest(
    Guid RecipientId,
    string? NewLogin = null,
    string? NewPassword = null,
    int? NewRecipientTimeUtcOffsetHours = null
);