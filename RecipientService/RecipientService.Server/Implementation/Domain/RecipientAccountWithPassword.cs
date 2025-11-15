using System;

namespace Manager.RecipientService.Server.Implementation.Domain;

public record RecipientAccountWithPassword(
    Guid Id,
    string Login,
    string Password,
    RecipientAccountState State,
    TimeZoneInfo TimeZoneInfo,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
) : RecipientAccount(Id, Login, State, TimeZoneInfo, CreatedAtUtc, UpdatedAtUtc);