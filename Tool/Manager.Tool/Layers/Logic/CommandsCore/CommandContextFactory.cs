using System.Collections.Generic;
using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandContextFactory : ICommandContextFactory
{
    public CommandContext Create(string[] arguments)
    {
        var options = new List<CommandOption>();
        foreach (var argument in arguments)
        {
            if (argument.StartsWith('-'))
            {
                options.Add(new CommandOption(argument));
            }
            else
            {
                options[^1] = new CommandOption(options[^1].Argument, argument);
            }
        }

        return new CommandContext(new User(), options.ToArray());
    }
}