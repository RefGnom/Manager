using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic;

public class HelpCommand : IToolCommand
{
    public string CommandName => "help";
    public string Description => "Get description for spaces and commands. Example: manager help";
    public CommandSpace? CommandSpace => null;
}