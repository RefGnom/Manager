using System;

namespace Manager.RecipientService.Server.Implementation.Domain;

public record RecipientAccountWithPasswordHash(
    Guid Id,
    string Login,
    string PasswordHash,
    RecipientAccountState State,
    TimeZoneInfo TimeZoneInfo,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
) : RecipientAccount(Id, Login, State, TimeZoneInfo, CreatedAtUtc, UpdatedAtUtc);