﻿using System;
using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Dao.Api.Responses;
using Manager.RecipientService.Server.Implementation;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Api;

public interface IRecipientAccountConverter
{
    CreateRecipientAccountDto ToDto(CreateRecipientAccountRequest request);
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

    public UpdateRecipientAccountDto ToDto(PatchRecipientAccountRequest request)
    {
        return new UpdateRecipientAccountDto(
            request.RecipientId,
            request.NewLogin,
            request.NewPassword,
            request.NewRecipientTimeUtcOffsetHours.HasValue
                ? TimeSpan.FromHours(request.NewRecipientTimeUtcOffsetHours.Value)
                : null
        );
    }

    public RecipientAccountResponse ToResponse(RecipientAccount recipientAccount) => new(
        recipientAccount.Id,
        recipientAccount.Login,
        recipientAccount.State.AccountState,
        recipientAccount.State.StateReason,
        recipientAccount.TimeZoneInfo.BaseUtcOffset
    );
}