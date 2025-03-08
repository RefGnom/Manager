using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Extensions.LinqExtensions;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;
using Manager.Tool.Layers.Presentation;

namespace Manager.Tool.Layers.Logic;

public class HelpCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IToolLogger<HelpCommand> logger,
    IEnumerable<IToolCommand> toolCommands,
    IToolWriter toolWriter
) : CommandExecutorBase<HelpCommand>(toolCommandFactory, logger)
{
    private readonly IToolCommand[] _toolCommands = toolCommands.ToArray();
    private readonly IToolWriter _toolWriter = toolWriter;

    protected override Task ExecuteAsync(CommandContext context, HelpCommand command)
    {
        var commandsGroupedBySpace = _toolCommands.GroupBy(x => x.CommandSpace, new CommandSpaceEqualityComparer());
        foreach (var spaceGroup in commandsGroupedBySpace)
        {
            _toolWriter.WriteMessage(spaceGroup.Key?.Description ?? "Common");
            spaceGroup.ToArray().Foreach(c => _toolWriter.WriteToolCommand(c, true));
        }

        return Task.CompletedTask;
    }
}