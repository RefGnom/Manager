using System;
using Manager.Core.Common.HelperObjects.Result;

namespace Manager.RecipientService.Server.Implementation.Domain;

public record RecipientAccount(
    Guid Id,
    string Login,
    RecipientAccountState State,
    TimeZoneInfo TimeZoneInfo,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
)
{
    public Result<RecipientAccount, string> Activate()
    {
        if (State.AccountState == AccountState.Active)
        {
            return "Аккаунт уже активирован";
        }

        return this with { State = new RecipientAccountState(AccountState.Active, StateReason.ActivatedByAdmin) };
    }
};