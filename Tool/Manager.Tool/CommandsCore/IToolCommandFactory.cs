namespace Manager.Tool.CommandsCore;

public interface IToolCommandFactory
{
    TCommand CreateCommand<TCommand>()
        where TCommand : IToolCommand, new();
}