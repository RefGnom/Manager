using Manager.Core.Common.Factories;
using Manager.Core.Common.Hashing;
using Manager.Core.Common.String;
using Manager.Core.Common.Time;
using Manager.RecipientService.Server.Implementation.Domain;
using Manager.RecipientService.Server.Implementation.UseCase.Dto;

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
    IHasher hasher,
    IRecipientAccountStateFactory recipientAccountStateFactory,
    ITimeZoneInfoFactory timeZoneInfoFactory,
    IDateTimeProvider dateTimeProvider
) : IRecipientAccountFactory
{
    public RecipientAccountWithPasswordHash Create(CreateRecipientAccountDto createRecipientAccountDto) => new(
        guidFactory.Create(),
        createRecipientAccountDto.Login,
        hasher.Hash(createRecipientAccountDto.Password),
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
        updateRecipientAccountDto.NewPassword.IsNullOrEmpty()
            ? recipientAccount.PasswordHash
            : hasher.Hash(updateRecipientAccountDto.NewPassword),
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