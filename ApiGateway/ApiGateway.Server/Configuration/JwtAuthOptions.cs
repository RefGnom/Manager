using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Manager.ApiGateway.Server.Configuration;

public static class JwtAuthOptions
{
    private const string Key = "secret_key_ca938a35-a54e-480a-9ff7-cb4e7f985afb";

    public const string Audience = "ApiGateway.Server";
    public const string Issuer = "ApiGateway.Server";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}