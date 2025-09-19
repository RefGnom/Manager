using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.Api.Converters;
using Manager.AuthenticationService.Server.Layers.Api.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Manager.AuthenticationService.Server.Layers.Api.Controllers;

[ApiController]
[AuthorizationResource("AuthorizationModel")]
[Route("api/authorization-model")]
public class AuthorizationModelController(
    IAuthorizationModelService authorizationModelService,
    IAuthorizationConverter authorizationConverter
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAuthorizationModel(
        [FromBody] CreateAuthorizationModelRequest createAuthorizationModelRequest
    )
    {
        var authorizationModelDto = authorizationConverter.ToDto(createAuthorizationModelRequest);
        var createResult = await authorizationModelService.CreateAsync(authorizationModelDto);

        if (createResult.IsSuccess)
        {
            return Created(string.Empty, createResult.Value);
        }

        if (createResult.Error is CreateAuthorizationModelErrorCode.AuthorizationModelAlreadyExists)
        {
            return Conflict(createResult.Error.GetDescription());
        }

        return BadRequest(createResult.Error.GetDescription());
    }
}