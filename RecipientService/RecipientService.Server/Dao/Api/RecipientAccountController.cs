using System;
using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Dao.Api.Responses;
using Manager.RecipientService.Server.Implementation;
using Manager.RecipientService.Server.Implementation.UseCase.Statuses;
using Microsoft.AspNetCore.Mvc;

namespace Manager.RecipientService.Server.Dao.Api;

[ApiController]
[AuthorizationResource("RecipientAccount")]
[Route("api/recipients")]
public class RecipientAccountController(
    IRecipientAccountService recipientAccountService,
    IRecipientAccountConverter recipientAccountConverter
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateRecipientAccount([FromBody] CreateRecipientAccountRequest request)
    {
        var createRecipientAccountDto = recipientAccountConverter.ToDto(request);
        var createResult = await recipientAccountService.CreateAsync(createRecipientAccountDto);
        if (createResult.IsSuccess)
        {
            return Created(string.Empty, recipientAccountConverter.ToResponse(createResult.Value));
        }

        return createResult.Status == CreateAccountStatus.LoginAlreadyExists
            ? Conflict(createResult.Error)
            : BadRequest(createResult.Error);
    }

    [HttpPost("{recipientId:guid}/activate")]
    public async Task<IActionResult> ActivateRecipientAccount([FromRoute] Guid recipientId)
    {
        var activateResult = await recipientAccountService.ActivateAsync(recipientId);
        return activateResult.Status switch
        {
            ActivateAccountStatus.Activated => Ok(),
            ActivateAccountStatus.NotFound => NotFound(),
            ActivateAccountStatus.Rejected => BadRequest(
                ErrorResponse.Create("Rejected", $"активация запрещена по причине: {activateResult.Error}")
            ),
            _ => throw new Exception($"Unknown activation status `{activateResult.Status}`"),
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginRecipientAccount([FromBody] LoginRecipientAccountRequest request)
    {
        var recipientAccountCredentials = recipientAccountConverter.ToDto(request);
        var loginResult = await recipientAccountService.LoginAsync(recipientAccountCredentials);

        return loginResult.Status switch
        {
            LoginAccountStatus.Success => Ok(LoginRecipientAccountResponse.CreateSuccess()),
            LoginAccountStatus.NotFound => NotFound(LoginRecipientAccountResponse.CreateNotFound(loginResult.Error)),
            LoginAccountStatus.Deleted => Ok(LoginRecipientAccountResponse.CreateDeleted(loginResult.Error)),
            LoginAccountStatus.LoginRejected => Ok(LoginRecipientAccountResponse.CreateRejected(loginResult.Error)),
            _ => throw new Exception($"Unknown login status {loginResult.Status}"),
        };
    }

    [HttpGet("{recipientId:guid}")]
    public async Task<IActionResult> GetRecipientAccount([FromRoute] Guid recipientId)
    {
        var recipientAccount = await recipientAccountService.FindAsync(recipientId);
        if (recipientAccount is null)
        {
            return NotFound();
        }

        return Ok(recipientAccountConverter.ToResponse(recipientAccount));
    }

    [HttpPatch("{recipientId:guid}")]
    public async Task<IActionResult> PatchRecipientAccount(
        [FromRoute] Guid recipientId,
        [FromBody] PatchRecipientAccountRequest request
    )
    {
        request.Id = recipientId;
        var updateRecipientAccountDto = recipientAccountConverter.ToDto(request);
        var updateResult = await recipientAccountService.UpdateAsync(updateRecipientAccountDto);
        if (updateResult.IsSuccess)
        {
            return Ok();
        }

        return updateResult.Status == UpdateAccountStatus.NotFound
            ? NotFound(updateResult.Error)
            : BadRequest(updateResult.Error);
    }

    [HttpDelete("{recipientId:guid}")]
    public async Task<IActionResult> DeleteRecipientAccount([FromRoute] Guid recipientId)
    {
        var deleteResult = await recipientAccountService.DeleteAsync(recipientId);
        if (deleteResult.IsSuccess)
        {
            return Ok();
        }

        return deleteResult.Status == DeleteAccountStatus.NotFound
            ? NotFound(deleteResult.Error)
            : BadRequest(deleteResult.Error);
    }
}