using System;
using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Dao.Api.Responses;
using Manager.RecipientService.Server.Implementation.Domain;
using Manager.RecipientService.Server.Implementation.UseCase.Dto;

namespace Manager.RecipientService.Server.Dao.Api;

public interface IRecipientAccountConverter
{
    CreateRecipientAccountDto ToDto(CreateRecipientAccountRequest request);
    RecipientAccountCredentials ToDto(LoginRecipientAccountRequest request);
    UpdateRecipientAccountDto ToDto(PatchRecipientAccountRequest request);
    RecipientAccountResponse ToResponse(RecipientAccount recipientAccount);
}

public class RecipientAccountConverter : IRecipientAccountConverter
{
    public CreateRecipientAccountDto ToDto(CreateRecipientAccountRequest request) => new(
        request.Login,
        request.Password,
        TimeSpan.FromHours(request.RecipientTimeUtcOffsetHours)
    );

    public RecipientAccountCredentials ToDto(LoginRecipientAccountRequest request) =>
        new(request.Login, request.Password);

    public UpdateRecipientAccountDto ToDto(PatchRecipientAccountRequest request) => new(
        request.Id,
        request.NewLogin,
        request.NewPassword,
        request.NewRecipientTimeUtcOffsetHours.HasValue
            ? TimeSpan.FromHours(request.NewRecipientTimeUtcOffsetHours.Value)
            : null
    );

    public RecipientAccountResponse ToResponse(RecipientAccount recipientAccount) => new(
        recipientAccount.Id,
        recipientAccount.Login,
        recipientAccount.State.AccountState,
        recipientAccount.State.StateReason,
        recipientAccount.TimeZoneInfo.BaseUtcOffset
    );
}