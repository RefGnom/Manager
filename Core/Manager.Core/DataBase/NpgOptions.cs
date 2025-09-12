using Microsoft.Extensions.Logging;

namespace Manager.Core.DataBase;

public class NpgOptions
{
    public required string ConnectionString { get; set; }
    public required LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public required bool EnableSensitiveDataLogging { get; set; } = false;
}