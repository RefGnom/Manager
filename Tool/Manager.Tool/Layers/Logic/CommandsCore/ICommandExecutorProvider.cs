namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface ICommandExecutorProvider
{
    ICommandExecutor GetForCommand(IToolCommand command);
}