using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommand : IToolCommand
{
    public string Command => "start";
    public string CommandSpace => "timers";
}