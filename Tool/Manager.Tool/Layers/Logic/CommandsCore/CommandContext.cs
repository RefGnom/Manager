using System.Linq;
using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public record CommandContext(
    User User,
    bool IsAuthenticated,
    CommandSpace CommandSpace,
    string CommandName,
    CommandFlag[] Flags
)
{
    public bool IsDebugMode { get; } = Flags.Any(x => x.Argument == "-d");
}