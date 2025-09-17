using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class ResetTimerCommand : IToolCommand
{
    public string CommandName => "reset";
    public string Description => "Reset timer and make previous timer archived.";
    public string Example => "manager timers reset <timer_name>";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}