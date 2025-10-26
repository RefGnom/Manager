using System;
using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Dao.Api.Responses;
using Manager.RecipientService.Server.Implementation;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Api;

public interface IRecipientAccountConverter
{
    CreateRecipientAccountDto ToDto(CreateRecipientAccountRequest createRecipientAccountRequest);
    RecipientAccountResponse ToResponse(RecipientAccount recipientAccount);
}

public class RecipientAccountConverter : IRecipientAccountConverter
{
    public CreateRecipientAccountDto ToDto(CreateRecipientAccountRequest createRecipientAccountRequest) => new(
        createRecipientAccountRequest.Login,
        createRecipientAccountRequest.Password,
        TimeSpan.FromHours(createRecipientAccountRequest.RecipientTimeUtcOffsetHours)
    );

    public RecipientAccountResponse ToResponse(RecipientAccount recipientAccount) => new(
        recipientAccount.Id,
        recipientAccount.Login,
        recipientAccount.State.AccountState,
        recipientAccount.State.StateReason,
        recipientAccount.TimeZoneInfo.BaseUtcOffset
    );
}