using System.ComponentModel.DataAnnotations;

namespace Manager.Core.AppConfiguration.Authentication;

public class AuthenticationServiceSetting
{
    [Required]
    public required string ApiKey { get; set; }
}