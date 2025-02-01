using System.Collections.Generic;
using System.Linq;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandContextFactory(IUserProvider userProvider) : ICommandContextFactory
{
    private readonly IUserProvider _userProvider = userProvider;

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

        var user = _userProvider.GetUser();
        return new CommandContext(user, commandSpace, spacesAndCommand.Last(), flags.ToArray());
    }
}