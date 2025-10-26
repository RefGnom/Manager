using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Implementation;
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
}