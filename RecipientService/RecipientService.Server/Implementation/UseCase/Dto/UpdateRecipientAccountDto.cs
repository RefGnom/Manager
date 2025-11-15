using System;

namespace Manager.RecipientService.Server.Implementation.UseCase.Dto;

public record UpdateRecipientAccountDto(
    Guid Id,
    string? NewLogin,
    string? NewPassword,
    TimeSpan? NewRecipientTimeUtcOffset
);