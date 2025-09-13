using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Manager.Core.AppConfiguration.DataBase;

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
    public required LogLevel LogLevel { get; set; }

    [Required]
    public required bool EnableSensitiveDataLogging { get; set; }
}