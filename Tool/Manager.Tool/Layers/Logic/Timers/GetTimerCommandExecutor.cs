using System;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class GetTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory) : CommandExecutorBase<GetTimerCommand>(toolCommandFactory)
{
    protected override Task ExecuteAsync(CommandContext context, GetTimerCommand command)
    {
        throw new NotImplementedException();
    }
}