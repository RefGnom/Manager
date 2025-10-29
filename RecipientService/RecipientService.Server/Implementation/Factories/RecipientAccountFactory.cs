using Manager.Core.Common.Factories;
using Manager.Core.Common.Time;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Implementation.Factories;

public interface IRecipientAccountFactory
{
    RecipientAccountWithPasswordHash Create(CreateRecipientAccountDto createRecipientAccountDto);

    RecipientAccountWithPasswordHash Create(
        RecipientAccountWithPasswordHash recipientAccount,
        UpdateRecipientAccountDto updateRecipientAccountDto
    );

    RecipientAccountWithPasswordHash CreateDeleted(RecipientAccountWithPasswordHash recipientAccount);
}

public class RecipientAccountFactory(
    IGuidFactory guidFactory,
    IRecipientAccountStateFactory recipientAccountStateFactory,
    ITimeZoneInfoFactory timeZoneInfoFactory,
    IDateTimeProvider dateTimeProvider
) : IRecipientAccountFactory
{
    public RecipientAccountWithPasswordHash Create(CreateRecipientAccountDto createRecipientAccountDto) => new(
        guidFactory.Create(),
        createRecipientAccountDto.Login,
        createRecipientAccountDto.Password,
        recipientAccountStateFactory.CreateInactiveByNewUser(),
        timeZoneInfoFactory.CreateByOffset(createRecipientAccountDto.RecipientTimeUtcOffset),
        dateTimeProvider.UtcNow,
        dateTimeProvider.UtcNow
    );

    public RecipientAccountWithPasswordHash Create(
        RecipientAccountWithPasswordHash recipientAccount,
        UpdateRecipientAccountDto updateRecipientAccountDto
    ) => new(
        recipientAccount.Id,
        updateRecipientAccountDto.NewLogin ?? recipientAccount.Login,
        updateRecipientAccountDto.NewPassword ?? recipientAccount.PasswordHash,
        recipientAccount.State,
        updateRecipientAccountDto.NewRecipientTimeUtcOffset.HasValue
            ? timeZoneInfoFactory.CreateByOffset(updateRecipientAccountDto.NewRecipientTimeUtcOffset.Value)
            : recipientAccount.TimeZoneInfo,
        recipientAccount.CreatedAtUtc,
        dateTimeProvider.UtcNow
    );

    public RecipientAccountWithPasswordHash CreateDeleted(RecipientAccountWithPasswordHash recipientAccount) =>
        recipientAccount with { State = recipientAccountStateFactory.CreateDeleted() };
}