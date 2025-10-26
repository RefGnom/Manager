using Manager.Core.Common.Factories;
using Manager.Core.Common.Time;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Implementation.Factories;

public interface IRecipientAccountFactory
{
    RecipientAccountWithPasswordHash Create(CreateRecipientAccountDto createRecipientAccountDto);
}

public class RecipientAccountFactory(
    IGuidFactory guidFactory,
    IRecipientAccountStateFactory recipientAccountStateFactory,
    ITimeZoneInfoFactory timeZoneInfoFactory,
    IDateTimeProvider dateTimeProvider
) : IRecipientAccountFactory
{
    public RecipientAccountWithPasswordHash Create(CreateRecipientAccountDto createRecipientAccountDto)
    {
        return new RecipientAccountWithPasswordHash(
            guidFactory.Create(),
            createRecipientAccountDto.Login,
            createRecipientAccountDto.Password,
            recipientAccountStateFactory.CreateInactiveByNewUser(),
            timeZoneInfoFactory.CreateByOffset(createRecipientAccountDto.RecipientTimeUtcOffset),
            dateTimeProvider.UtcNow,
            dateTimeProvider.UtcNow
        );
    }
}