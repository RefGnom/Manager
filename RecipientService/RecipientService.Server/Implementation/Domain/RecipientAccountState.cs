using System;
using Manager.Core.Common.Enum;

namespace Manager.RecipientService.Server.Implementation.Domain;

public record RecipientAccountState(
    AccountState AccountState,
    StateReason StateReason
)
{
    public sealed override string ToString() => $"'{AccountState.GetDescription()}, {StateReason.GetDescription()}'";
};

public record RecipientAccountStateWithId(
    Guid Id,
    AccountState AccountState,
    StateReason StateReason
) : RecipientAccountState(AccountState, StateReason);