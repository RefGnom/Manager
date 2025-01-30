namespace Manager.Tool.CommandsCore;

public class ToolCommandFactory : IToolCommandFactory
{
    public TCommand CreateCommand<TCommand>()
        where TCommand : IToolCommand, new()
    {
        return new TCommand();
    }
}