using System.Threading.Tasks;
using Manager.Core.Common.HelperObjects.Result;
using Manager.RecipientService.Server.Dao.Repository;
using Manager.RecipientService.Server.Implementation.Domain;
using Manager.RecipientService.Server.Implementation.Factories;

namespace Manager.RecipientService.Server.Implementation;

public interface IRecipientAccountService
{
    Task<ProcessResult<RecipientAccount, string, CreateAccountStatus>> CreateAsync(
        CreateRecipientAccountDto createRecipientAccountDto
    );
}

public class RecipientAccountService(
    IRecipientAccountRepository recipientAccountRepository,
    IRecipientAccountFactory recipientAccountFactory
) : IRecipientAccountService
{
    public async Task<ProcessResult<RecipientAccount, string, CreateAccountStatus>> CreateAsync(
        CreateRecipientAccountDto createRecipientAccountDto
    )
    {
        var foundRecipientAccount = await recipientAccountRepository.FindByLoginAsync(createRecipientAccountDto.Login);
        if (foundRecipientAccount is not null)
        {
            return ProcessResult<RecipientAccount, string, CreateAccountStatus>.Failure(
                "Аккаунт с таким логином уже существует",
                CreateAccountStatus.LoginAlreadyExists
            );
        }

        var recipientAccount = recipientAccountFactory.Create(createRecipientAccountDto);
        await recipientAccountRepository.CreateAsync(recipientAccount);
        return ProcessResult<RecipientAccount, string, CreateAccountStatus>.Ok(
            recipientAccount,
            CreateAccountStatus.Created
        );
    }
}