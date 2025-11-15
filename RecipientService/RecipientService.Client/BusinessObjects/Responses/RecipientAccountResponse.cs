using System;

namespace Manager.RecipientService.Client.BusinessObjects.Responses;

public record RecipientAccountResponse(
    Guid Id,
    string Login,
    AccountState AccountState,
    StateReason StateReason,
    TimeSpan RecipientTimeUtcOffset
);