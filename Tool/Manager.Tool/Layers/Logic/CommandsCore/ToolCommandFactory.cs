namespace Manager.Tool.Layers.Logic.CommandsCore;

public class ToolCommandFactory : IToolCommandFactory
{
    public TCommand CreateCommand<TCommand>()
        where TCommand : IToolCommand, new()
    {
        return new TCommand();
    }
}