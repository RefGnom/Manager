using System;

namespace Manager.RecipientService.Server.Implementation;

public record CreateRecipientAccountDto(
    string Login,
    string Password,
    TimeSpan RecipientTimeUtcOffset
);

public record UpdateRecipientAccountDto(
    Guid Id,
    string? NewLogin,
    string? NewPassword,
    TimeSpan? NewRecipientTimeUtcOffset
);