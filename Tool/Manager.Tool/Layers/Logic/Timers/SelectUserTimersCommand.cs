using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class SelectUserTimersCommand : IToolCommand
{
    public string CommandName => "my";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}