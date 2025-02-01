using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommand : IToolCommand
{
    public string CommandName => "start";
    public CommandSpace CommandSpace { get; } = new("timers");
}