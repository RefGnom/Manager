using System.ComponentModel.DataAnnotations;

namespace Manager.Core.AppConfiguration.Authentication;

public class AuthenticationSetting
{
    [Required]
    public string Resource { get; set; }
}