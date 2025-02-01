namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface IArgumentsValidator
{
    ValidationResult Validate(string[] arguments);
}