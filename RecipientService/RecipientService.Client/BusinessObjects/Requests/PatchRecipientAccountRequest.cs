using System;

namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public record PatchRecipientAccountRequest(
    Guid RecipientId,
    string? NewLogin,
    string? NewPassword,
    int? NewRecipientTimeUtcOffsetHours
);