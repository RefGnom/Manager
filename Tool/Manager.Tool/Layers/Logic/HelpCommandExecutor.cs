using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        const bool isDetailed = true;
        var commandsGroupedBySpace = _toolCommands.GroupBy(x => x.CommandSpace, new CommandSpaceEqualityComparer());
        foreach (var spaceGroup in commandsGroupedBySpace)
        {
            _toolWriter.WriteMessage(spaceGroup.Key?.Description ?? "Common");
            var spaceCommands = spaceGroup.ToArray();
            for (var i = 0; i < spaceCommands.Length; i++)
            {
                _toolWriter.WriteToolCommand(spaceCommands[i], isDetailed);
                if (isDetailed && spaceCommands[i].CommandOptions.Length > 0 && i + 1 < spaceCommands.Length)
                {
                    _toolWriter.MoveOnNextLine();
                }
            }

            _toolWriter.MoveOnNextLine();
        }

        return Task.CompletedTask;
    }
}