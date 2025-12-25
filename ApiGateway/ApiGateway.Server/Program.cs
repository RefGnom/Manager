using System.Text;
using Manager.ApiGateway.Server.Configuration;
using Manager.Core.AppConfiguration;
using Manager.Core.Common.DependencyInjection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.Logging.Configuration;
using Manager.RecipientService.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

SolutionRootEnvironmentVariablesLoader.Load();

var builder = WebApplication.CreateBuilder(args);
builder.AddCustomLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .UseAutoRegistrationForCoreNetworking()
    .UseAutoRegistrationForCoreCommon()
    .AddSingleton<IRecipientServiceApiClientFactory, RecipientServiceApiClientFactory>()
    .AddSingleton<IRecipientServiceApiClient>(x =>
        x.GetRequiredService<IRecipientServiceApiClientFactory>().Create("fake key")
    )
    .ConfigureOptionsWithValidation<ApiKeysOptions>()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtAuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtAuthOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = JwtAuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        }
    );

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms<ApiKeyTransformProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication().UseAuthorization();
app.MapControllers();
app.MapReverseProxy();
app.Run();

public class JwtAuthOptions
{
    private const string Key = "secret_key_ca938a35-a54e-480a-9ff7-cb4e7f985afb";

    public const string Audience = "ApiGateway.Server";
    public const string Issuer = "ApiGateway.Server";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}