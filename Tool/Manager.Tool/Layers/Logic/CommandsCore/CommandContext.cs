using System.Linq;
using Manager.AuthenticationService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public record CommandContext(
    User? User,
    string[] Arguments,
    CommandOption[] Options
)
{
    public bool IsDebugMode { get; } = Options.Any(x => x.Argument == "-d");
}