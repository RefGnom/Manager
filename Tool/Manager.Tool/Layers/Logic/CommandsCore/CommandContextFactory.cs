using System.Collections.Generic;
using System.Linq;
using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandContextFactory : ICommandContextFactory
{
    public CommandContext Create(string[] arguments)
    {
        var flags = new List<CommandFlag>();
        var spacesAndCommand = arguments.TakeWhile(x => !x.StartsWith('-'))
            .ToArray();
        var commandSpace = new CommandSpace(spacesAndCommand.SkipLast(1).ToArray());

        foreach (var flag in arguments.SkipWhile(x => !x.StartsWith('-')).ToArray())
        {
            if (flag.StartsWith('-'))
            {
                flags.Add(new CommandFlag(flag));
            }
            else
            {
                flags[^1] = new CommandFlag(flags[^1].Argument, flag);
            }
        }

        return new CommandContext(new User(), commandSpace, spacesAndCommand.Last(), flags.ToArray());
    }
}