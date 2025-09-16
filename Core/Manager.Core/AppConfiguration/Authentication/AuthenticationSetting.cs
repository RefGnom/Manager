using System.ComponentModel.DataAnnotations;

namespace Manager.Core.AppConfiguration.Authentication;

public class AuthenticationSetting
{
    [Required]
    public required string Resource { get; set; }
    public bool Disabled { get; set; }
}