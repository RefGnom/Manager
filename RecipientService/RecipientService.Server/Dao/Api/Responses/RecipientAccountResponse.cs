using System;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Api.Responses;

public record RecipientAccountResponse(
    Guid Id,
    string Login,
    AccountState AccountState,
    StateReason StateReason,
    TimeSpan RecipientTimeUtcOffset
);