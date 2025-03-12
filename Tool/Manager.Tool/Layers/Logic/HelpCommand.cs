using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic;

public class HelpCommand : IToolCommand
{
    public string CommandName => "help";
    public string Description => "Get description for spaces and commands.";
    public string Example => "manager help, manager timers help, manager timers start help";
    public CommandSpace? CommandSpace => null;
}