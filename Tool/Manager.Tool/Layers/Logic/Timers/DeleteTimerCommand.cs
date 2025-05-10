using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class DeleteTimerCommand : IToolCommand
{
    public string CommandName => "delete";
    public string Description => "Mark timer as deleted.";
    public string Example => "manager timers delete <timer_name>";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}