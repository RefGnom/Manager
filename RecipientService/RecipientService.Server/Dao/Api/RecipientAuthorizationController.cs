using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace Manager.RecipientService.Server.Dao.Api;

[ApiController]
[AuthorizationResource("RecipientAuthorization")]
[Route("api/recipient-authorization")]
public class RecipientAuthorizationController(
    IRecipientAuthorizationService recipientAuthorizationService,
    IRecipientAuthorizationConverter recipientAuthorizationConverter
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAuthorization(
        [FromRoute] RecipientAuthorizationRequest request
    )
    {
        var foundRecipientAuthorization = await recipientAuthorizationService.FindRecipientAuthorizationAsync(
            request.RecipientId,
            request.RequestedService,
            request.RequestedResource
        );

        if (!foundRecipientAuthorization.HasValue)
        {
            return NotFound();
        }

        return Ok(recipientAuthorizationConverter.ToResponse(request, foundRecipientAuthorization.Value));
    }
}