using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Manager.RecipientService.Client;
using Manager.RecipientService.Client.BusinessObjects.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Manager.ApiGateway.Server;

[ApiController]
[Route("login")]
public class LoginController(
    IRecipientServiceApiClient recipientServiceApiClient
) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> LoginAsync([FromBody] LoginRequest request)
    {
        var loginRequest = new LoginRecipientAccountRequest(request.Login, request.Password);
        var loginHttpResult = await recipientServiceApiClient.LoginRecipientAccountAsync(loginRequest);
        if (!loginHttpResult.IsSuccess)
        {
            return Results.StatusCode((int)loginHttpResult.StatusCode);
        }

        var claims = new List<Claim> { new(ClaimTypes.Name, request.Login) };
        var jwt = new JwtSecurityToken(
            issuer: JwtAuthOptions.Issuer,
            audience: JwtAuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
            signingCredentials: new SigningCredentials(
                JwtAuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256
            )
        );

        return Results.Json(new
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(jwt),
            username = request.Login,
        });
    }
}