namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface IConcreteCommandMatcher
{
    bool CanMatch(IToolCommand command);
    MatchCommandResult MatchCommandForContext(IToolCommand command, CommandContext context);
}