using System;
using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Implementation;
using Manager.RecipientService.Server.Implementation.UseCase.Statuses;
using Microsoft.AspNetCore.Mvc;

namespace Manager.RecipientService.Server.Dao.Api;

[ApiController]
[AuthorizationResource("RecipientAccount")]
[Route("api/recipient-account")]
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

    [HttpGet]
    public async Task<IActionResult> GetRecipientAccount([FromRoute] Guid recipientId)
    {
        var recipientAccount = await recipientAccountService.FindAsync(recipientId);
        if (recipientAccount is null)
        {
            return NotFound();
        }

        return Ok(recipientAccountConverter.ToResponse(recipientAccount));
    }

    [HttpPatch]
    public async Task<IActionResult> PatchRecipientAccount([FromBody] PatchRecipientAccountRequest request)
    {
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

    [HttpDelete]
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