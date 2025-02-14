using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class ResetTimerCommand : IToolCommand
{
    public string CommandName => "reset";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}