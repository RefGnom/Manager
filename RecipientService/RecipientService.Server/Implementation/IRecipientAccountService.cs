using System;
using System.Threading.Tasks;
using Manager.Core.Common.HelperObjects.Result;
using Manager.RecipientService.Server.Dao.Repository;
using Manager.RecipientService.Server.Implementation.Domain;
using Manager.RecipientService.Server.Implementation.Factories;
using Manager.RecipientService.Server.Implementation.UseCase.Dto;
using Manager.RecipientService.Server.Implementation.UseCase.Statuses;

namespace Manager.RecipientService.Server.Implementation;

public interface IRecipientAccountService
{
    Task<ProcessResult<RecipientAccount, string, CreateAccountStatus>> CreateAsync(
        CreateRecipientAccountDto createRecipientAccountDto
    );

    Task<ProcessResult<string, ActivateAccountStatus>> ActivateAsync(Guid recipientId);
    Task<ProcessResult<string, LoginAccountStatus>> LoginAsync(RecipientAccountCredentials credentials);
    Task<RecipientAccount?> FindAsync(Guid recipientId);
    Task<ProcessResult<string, UpdateAccountStatus>> UpdateAsync(UpdateRecipientAccountDto updateRecipientAccountDto);
    Task<ProcessResult<string, DeleteAccountStatus>> DeleteAsync(Guid recipientId);
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

    public async Task<ProcessResult<string, ActivateAccountStatus>> ActivateAsync(Guid recipientId)
    {
        var foundRecipientAccount = await recipientAccountRepository.FindAsync(recipientId);
        if (foundRecipientAccount is null)
        {
            return ProcessResult<string, ActivateAccountStatus>.Failure(
                $"Не существует аккаунта с идентификатором {recipientId}",
                ActivateAccountStatus.NotFound
            );
        }

        var activateAccountResult = foundRecipientAccount.Activate();
        if (activateAccountResult.IsFailure)
        {
            return ProcessResult<string, ActivateAccountStatus>.Failure(
                activateAccountResult.Error,
                ActivateAccountStatus.Rejected
            );
        }

        await recipientAccountRepository.UpdateAsync(activateAccountResult.Value);
        return ProcessResult<string, ActivateAccountStatus>.Ok(ActivateAccountStatus.Activated);
    }

    public async Task<ProcessResult<string, LoginAccountStatus>> LoginAsync(RecipientAccountCredentials credentials)
    {
        var foundRecipientAccount = await recipientAccountRepository.FindByLoginAsync(credentials.Login);
        if (foundRecipientAccount is null)
        {
            return ProcessResult<string, LoginAccountStatus>.Failure(
                $"Не существует аккаунта с логином {credentials.Login}",
                LoginAccountStatus.NotFound
            );
        }

        return foundRecipientAccount.State.AccountState switch
        {
            AccountState.Active => ProcessResult<string, LoginAccountStatus>.Ok(LoginAccountStatus.Success),
            AccountState.Deleted => ProcessResult<string, LoginAccountStatus>.Failure(
                $"Аккаунт с логином {credentials.Login} удалён",
                LoginAccountStatus.Deleted
            ),
            _ => ProcessResult<string, LoginAccountStatus>.Failure(
                $"Аккаунт находится в состоянии {foundRecipientAccount.State}",
                LoginAccountStatus.LoginRejected
            ),
        };
    }

    public async Task<RecipientAccount?> FindAsync(Guid recipientId) =>
        await recipientAccountRepository.FindAsync(recipientId);

    public async Task<ProcessResult<string, UpdateAccountStatus>> UpdateAsync(
        UpdateRecipientAccountDto updateRecipientAccountDto
    )
    {
        var actualRecipientAccount = await recipientAccountRepository.FindAsync(updateRecipientAccountDto.Id);
        if (actualRecipientAccount is null)
        {
            return ProcessResult<string, UpdateAccountStatus>.Failure(
                $"Не существует аккаунта для получателя {updateRecipientAccountDto.Id}",
                UpdateAccountStatus.NotFound
            );
        }

        var updatedRecipientAccount = recipientAccountFactory.Create(actualRecipientAccount, updateRecipientAccountDto);
        await recipientAccountRepository.UpdateAsync(updatedRecipientAccount);
        return ProcessResult<string, UpdateAccountStatus>.Ok(
            UpdateAccountStatus.Updated
        );
    }

    public async Task<ProcessResult<string, DeleteAccountStatus>> DeleteAsync(Guid recipientId)
    {
        var actualRecipientAccount = await recipientAccountRepository.FindAsync(recipientId);
        if (actualRecipientAccount is null)
        {
            return ProcessResult<string, DeleteAccountStatus>.Failure(
                $"Не существует аккаунта для получателя {recipientId}",
                DeleteAccountStatus.NotFound
            );
        }

        var deletedRecipientAccount = recipientAccountFactory.CreateDeleted(actualRecipientAccount);
        await recipientAccountRepository.UpdateAsync(deletedRecipientAccount);
        return ProcessResult<string, DeleteAccountStatus>.Ok(
            DeleteAccountStatus.Deleted
        );
    }
}