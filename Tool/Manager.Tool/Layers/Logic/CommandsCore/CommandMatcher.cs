using System.Collections.Generic;
using System.Linq;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandMatcher(IEnumerable<IToolCommand> commands) : ICommandMatcher
{
    private readonly IToolCommand[] _commands = commands.ToArray();

    public MatchCommandResult GetMostSuitableForContext(CommandContext context)
    {
        return _commands.Select(x => MatchCommandForContext(x, context))
            .MaxBy(x => x.Score)!;
    }

    public MatchCommandResult MatchCommandForContext(IToolCommand command, CommandContext context)
    {
        var spaceLength = command.CommandSpace?.Values.Length ?? 0;
        var argumentsSpace = context.Arguments.Take(spaceLength).ToArray();
        if (command.CommandSpace is not null && !command.CommandSpace.Values.SequenceEqual(argumentsSpace))
        {
            return new MatchCommandResult(command, 0);
        }

        var commandName = context.Arguments.Skip(spaceLength).FirstOrDefault();
        if (command.CommandName != commandName)
        {
            return new MatchCommandResult(command, 0);
        }

        return new MatchCommandResult(command, 1);
    }
}