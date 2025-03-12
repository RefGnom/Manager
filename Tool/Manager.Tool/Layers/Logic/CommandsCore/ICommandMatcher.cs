namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface ICommandMatcher
{
    MatchCommandResult GetMostSuitableForContext(CommandContext context);
    MatchCommandResult MatchCommandForContext(IToolCommand command, CommandContext context);
}