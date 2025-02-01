namespace Manager.Tool.Layers.Logic.CommandsCore;

public class ArgumentsValidator : IArgumentsValidator
{
    public ValidationResult Validate(string[] arguments)
    {
        if (arguments.Length == 0)
        {
            return ValidationResult.Failure("Отсутствует команда");
        }

        if (arguments[0].StartsWith('-'))
        {
            return ValidationResult.Failure("Вводите в правильном порядке. Сначала команда, потом её аргументы");
        }

        return ValidationResult.Success();
    }
}