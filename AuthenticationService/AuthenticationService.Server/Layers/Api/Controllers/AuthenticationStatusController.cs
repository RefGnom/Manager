using System.Threading.Tasks;
using Manager.AuthenticationService.Server.Layers.Api.Converters;
using Manager.AuthenticationService.Server.Layers.Api.Responses;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.Core.AppConfiguration.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Manager.AuthenticationService.Server.Layers.Api.Controllers;

[ApiController]
[AuthorizationResource("AuthenticationStatus")]
[Route("api/authentication-status/{service}/{resource}")]
public class AuthenticationStatusController(
    IAuthenticationStatusService authenticationStatusService,
    IAuthenticationConverter authenticationConverter
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AuthenticationStatusResponse>> GetAuthenticationStatus(
        [FromRoute] [BindRequired] string service,
        [FromRoute] [BindRequired] string resource,
        [FromHeader(Name = "X-Caller-Api-Key")] [BindRequired]
        string apiKey
    )
    {
        var authenticationStatusDto = await authenticationStatusService.GetAsync(apiKey, service, resource);
        return authenticationConverter.ToResponse(authenticationStatusDto);
    }
}