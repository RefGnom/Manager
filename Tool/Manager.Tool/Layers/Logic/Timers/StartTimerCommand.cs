using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommand : IToolCommand
{
    public string CommandName => "start";
    public string Description => "Start new or stopped timer.";
    public string Example => "manager timers start <timer_nam>";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
    public CommandOptionInfo[] CommandOptions => [StartTimeOption, PingTimeoutOption];
    public CommandOptionInfo StartTimeOption { get; } = new("-t", "--start_time", "Time when need start timer");
    public CommandOptionInfo PingTimeoutOption { get; } = new(null, "--timeout", "After how long time notify about ending");
}