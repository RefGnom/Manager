using System;
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

    [HttpPost("{authorizationModelId:guid}/recreate")]
    public async Task<IActionResult> ReissueAuthorizationModel(Guid authorizationModelId, [FromQuery] int? daysAlive)
    {
        var authorizationModelDto = await authorizationModelService.FindAsync(authorizationModelId);
        if (authorizationModelDto == null)
        {
            return NotFound();
        }

        if (authorizationModelDto.State == AuthorizationModelState.Active)
        {
            return BadRequest("Authorization is active");
        }

        var reissueAuthorizationModel = await authorizationModelService.RecreateAsync(authorizationModelId, daysAlive);
        return Ok(reissueAuthorizationModel);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateAuthorizationModel(
        [FromBody] PatchAuthorizationModelRequest patchAuthorizationModelRequest
    )
    {
        var foundAuthorizationModelDto = await authorizationModelService.FindAsync(
            patchAuthorizationModelRequest.AuthorizationModelId
        );

        if (foundAuthorizationModelDto == null)
        {
            return NotFound();
        }

        var authorizationModelDto = authorizationConverter.Map(
            foundAuthorizationModelDto,
            patchAuthorizationModelRequest
        );
        await authorizationModelService.UpdateAsync(authorizationModelDto);

        return Ok();
    }

    [HttpPatch("{authorizationModelId:guid}/revoke")]
    public async Task<IActionResult> RevokeAuthorizationModel(Guid authorizationModelId)
    {
        var authorizationModelDto = await authorizationModelService.FindAsync(authorizationModelId);
        if (authorizationModelDto == null)
        {
            return NotFound();
        }

        if (authorizationModelDto.State != AuthorizationModelState.Active)
        {
            return BadRequest("AuthorizationModel is not active");
        }

        await authorizationModelService.RevokeAsync(authorizationModelId);
        return Ok();
    }

    [HttpGet("{authorizationModelId:guid}")]
    public async Task<IActionResult> GetAuthorizationModel([FromRoute] Guid authorizationModelId)
    {
        var foundAuthorizationModelDto = await authorizationModelService.FindAsync(authorizationModelId);

        if (foundAuthorizationModelDto == null)
        {
            return NotFound();
        }

        return Ok(foundAuthorizationModelDto);
    }

    [HttpDelete("{authorizationModelId:guid}")]
    public async Task<IActionResult> DeleteAuthorizationModel([FromRoute] Guid authorizationModelId)
    {
        var foundAuthorizationModelDto = await authorizationModelService.FindAsync(authorizationModelId);
        if (foundAuthorizationModelDto == null)
        {
            return NotFound();
        }

        await authorizationModelService.DeleteAsync(foundAuthorizationModelDto);
        return Ok();
    }
}