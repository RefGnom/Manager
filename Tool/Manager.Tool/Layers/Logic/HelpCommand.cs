using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic;

public class HelpCommand : IToolCommand
{
    public string CommandName => "help";
    public CommandSpace? CommandSpace => null;
}