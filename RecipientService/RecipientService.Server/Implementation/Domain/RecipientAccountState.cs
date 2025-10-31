using System;

namespace Manager.RecipientService.Server.Implementation.Domain;

public record RecipientAccountState(
    AccountState AccountState,
    StateReason StateReason
);

public record RecipientAccountStateWithId(
    Guid Id,
    AccountState AccountState,
    StateReason StateReason
) : RecipientAccountState(AccountState, StateReason);