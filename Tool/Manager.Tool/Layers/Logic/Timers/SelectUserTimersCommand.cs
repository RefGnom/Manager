using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class SelectUserTimersCommand : IToolCommand
{
    public string CommandName => "my";
    public string Description => "Get your timers. Example: manager timers my";
    public CommandSpace CommandSpace => TimerCommandConstants.TimersCommandSpace;
}