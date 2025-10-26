using System;

namespace Manager.RecipientService.Server.Implementation;

public record CreateRecipientAccountDto(
    string Login,
    string Password,
    TimeSpan RecipientTimeUtcOffset
);