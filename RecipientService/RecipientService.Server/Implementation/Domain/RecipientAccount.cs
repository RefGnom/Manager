using System;

namespace Manager.RecipientService.Server.Implementation.Domain;

public record RecipientAccount(
    Guid Id,
    string Login,
    string PasswordHash,
    RecipientAccountState State,
    TimeZoneInfo TimeZoneInfo,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);