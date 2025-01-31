namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface IToolCommandFactory
{
    TCommand CreateCommand<TCommand>()
        where TCommand : IToolCommand, new();
}