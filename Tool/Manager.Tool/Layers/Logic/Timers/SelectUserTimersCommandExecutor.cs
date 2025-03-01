using System;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class SelectUserTimersCommandExecutor(
    IToolCommandFactory toolCommandFactory) : CommandExecutorBase<SelectUserTimersCommand>(toolCommandFactory)
{
    protected override Task ExecuteAsync(CommandContext context, SelectUserTimersCommand command)
    {
        throw new NotImplementedException();
    }
}