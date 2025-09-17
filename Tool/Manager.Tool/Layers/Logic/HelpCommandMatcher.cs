using System.Linq;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic;

public class HelpCommandMatcher : IConcreteCommandMatcher
{
    public bool CanMatch(IToolCommand command)
    {
        return command is HelpCommand;
    }

    public MatchCommandResult MatchCommandForContext(IToolCommand command, CommandContext context)
    {
        var isHelpCommand = context.ContainsOption("-h", "--help") ||  context.Arguments.Contains("help");
        return new MatchCommandResult(command, isHelpCommand ? 1 : 0);
    }
}