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
        throw new System.NotImplementedException();
    }
}