using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommand : IToolCommand
{
    public string CommandName => "start";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
    public CommandOptionInfo[] CommandOptions => [StartTimeOption, PingTimeoutOption];
    public CommandOptionInfo StartTimeOption { get; } = new("-s", "--start_time", "Time when need timer start");
    public CommandOptionInfo PingTimeoutOption { get; } = new("-t", "--timeout", "After how long time notify about ending");
}