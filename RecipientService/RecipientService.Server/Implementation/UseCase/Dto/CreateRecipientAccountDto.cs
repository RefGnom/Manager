using System;

namespace Manager.RecipientService.Server.Implementation.UseCase.Dto;

public record CreateRecipientAccountDto(
    string Login,
    string Password,
    TimeSpan RecipientTimeUtcOffset
);