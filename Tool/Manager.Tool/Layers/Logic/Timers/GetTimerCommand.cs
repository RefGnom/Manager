using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class GetTimerCommand : IToolCommand
{
    public string CommandName => "get";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}