using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class StopTimerCommand : IToolCommand
{
    public string CommandName => "stop";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}