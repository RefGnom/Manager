using System.Collections.Generic;
using System.Linq;
using Manager.Tool.Layers.Logic.Authentication;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandContextFactory(IUserService userService) : ICommandContextFactory
{
    private readonly IUserService _userService = userService;

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

        var isAuthenticated = _userService.TryGetUser(out var user);
        return new CommandContext(user, isAuthenticated, commandSpace, spacesAndCommand.Last(), flags.ToArray());
    }
}