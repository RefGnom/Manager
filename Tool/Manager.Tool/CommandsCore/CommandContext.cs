using ManagerService.Client.ServiceModels;

namespace Manager.Tool.CommandsCore;

public record CommandContext(
    User User,
    CommandOption[] Options
)
{
    public bool IsDebugMode { get; } = Options.Any(x => x.Argument == "-d");
}