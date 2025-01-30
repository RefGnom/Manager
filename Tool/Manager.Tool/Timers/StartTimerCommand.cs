using Manager.Tool.CommandsCore;

namespace Manager.Tool.Timers;

public class StartTimerCommand : IToolCommand
{
    public string Command => "start";
    public string CommandSpace => "timers";
}