using System.ComponentModel.DataAnnotations;

namespace Manager.Core.EFCore;

public class DataBaseOptions
{
    [Required]
    public required string ConnectionStringTemplate { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    public string ConnectionString => string.Format(ConnectionStringTemplate, Username, Password);

    [Required]
    public required bool EnableSensitiveDataLogging { get; set; }
}