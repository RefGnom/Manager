using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic;

public class HelpCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ILogger<HelpCommand> logger,
    IEnumerable<IToolCommand> toolCommands
) : CommandExecutorBase<HelpCommand>(toolCommandFactory, logger)
{
    private readonly IToolCommand[] toolCommands = toolCommands.ToArray();
    private readonly ILogger<HelpCommand> logger = logger;

    protected override Task ExecuteAsync(CommandContext context, HelpCommand command)
    {
        const bool isDetailed = true;
        var commandsGroupedBySpace = toolCommands.GroupBy(x => x.CommandSpace, new CommandSpaceEqualityComparer());
        foreach (var spaceGroup in commandsGroupedBySpace)
        {
            logger.WriteMessage(spaceGroup.Key?.Description ?? "Common");
            var spaceCommands = spaceGroup.ToArray();
            for (var i = 0; i < spaceCommands.Length; i++)
            {
                logger.WriteToolCommand(spaceCommands[i], isDetailed);
                if (isDetailed && spaceCommands[i].CommandOptions.Length > 0 && i + 1 < spaceCommands.Length)
                {
                    logger.MoveOnNextLine();
                }
            }

            logger.MoveOnNextLine();
        }

        return Task.CompletedTask;
    }
}