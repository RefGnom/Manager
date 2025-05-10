using System.Linq;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public static class ToolCommandExtensions
{
    public static bool CanExecuteForContext(this IToolCommand command, CommandContext context)
    {
        var spaceLength = command.CommandSpace?.Values.Length ?? 0;
        var argumentsSpace = context.Arguments.Take(spaceLength).ToArray();
        if (command.CommandSpace is not null && !command.CommandSpace.Values.SequenceEqual(argumentsSpace))
        {
            return false;
        }

        var commandName = context.Arguments.Skip(spaceLength).FirstOrDefault();
        return command.CommandName == commandName;
    }
}