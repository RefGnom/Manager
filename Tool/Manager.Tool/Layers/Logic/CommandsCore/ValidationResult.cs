namespace Manager.Tool.Layers.Logic.CommandsCore;

public record ValidationResult(
    bool IsSuccess,
    string FailureMessage = ""
)
{
    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    public static ValidationResult Failure(string message)
    {
        return new ValidationResult(false, message);
    }
}