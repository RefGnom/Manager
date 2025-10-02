using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class StopTimerCommand : IToolCommand
{
    public CommandOptionInfo StopTimeOption { get; } = new("-t", "--stop_time", "Time when need stop timer");
    public string CommandName => "stop";
    public string Description => "Stop started timer.";
    public string Example => "manager timers stop <timer_name>";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
    public CommandOptionInfo[] CommandOptions => [StopTimeOption];
}