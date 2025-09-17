using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.Api.Converters;
using Manager.AuthenticationService.Server.Layers.Api.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Manager.AuthenticationService.Server.Layers.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/authorization-model")]
public class AuthorizationModelController(
    IAuthorizationModelService authorizationModelService,
    IAuthorizationConverter authorizationConverter
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAuthorizationModel(
        [FromBody] AuthorizationModelRequest model,
        [FromHeader(Name = "X-Api-Key-Hash"), BindRequired]
        string apiKeyHash
    )
    {
        var authorizationModelDto = authorizationConverter.ToDto(model, apiKeyHash);
        var createResult = await authorizationModelService.CreateAsync(authorizationModelDto);

        if (createResult.IsSuccess)
        {
            return Created();
        }

        if (createResult.Error is CreateAuthorizationModelErrorCode.AuthorizationModelAlreadyExists)
        {
            return Conflict(createResult.Error.GetDescription());
        }

        return BadRequest(createResult.Error.GetDescription());
    }
}