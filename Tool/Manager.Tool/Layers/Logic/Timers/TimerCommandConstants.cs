using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public static class TimerCommandConstants
{
    public const string DefaultTimerName = "default";
    public static CommandSpace TimersCommandSpace => new("timers");
}