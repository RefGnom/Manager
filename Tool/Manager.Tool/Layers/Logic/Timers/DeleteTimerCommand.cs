using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class DeleteTimerCommand : IToolCommand
{
    public string CommandName => "delete";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}