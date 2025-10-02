namespace Manager.Tool.Layers.Logic.CommandsCore;

public record ValidationResult(
    bool IsSuccess,
    string FailureMessage = ""
)
{
    public static ValidationResult Success() => new(true);

    public static ValidationResult Failure(string message) => new(false, message);
}