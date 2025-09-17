using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class GetTimerCommand : IToolCommand
{
    public string CommandName => "get";
    public string Description => "Get timer by name.";
    public string Example => "manager timers get <timer_name>";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}