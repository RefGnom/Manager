using System.Collections.Generic;
using System.Linq;
using Manager.Tool.Layers.Logic.Authentication;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandContextFactory(IUserService userService) : ICommandContextFactory
{
    private readonly IUserService _userService = userService;

    public CommandContext Create(string[] arguments)
    {
        var options = new List<CommandOption>();
        var commandArguments = arguments.TakeWhile(x => !x.StartsWith('-')).ToArray();

        foreach (var option in arguments.SkipWhile(x => !x.StartsWith('-')).ToArray())
        {
            if (option.StartsWith('-'))
            {
                options.Add(new CommandOption(option));
            }
            else
            {
                options[^1] = new CommandOption(options[^1].Argument, option);
            }
        }

        var user = _userService.FindUser();
        return new CommandContext(user, commandArguments, options.ToArray());
    }
}