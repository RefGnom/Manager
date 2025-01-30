namespace Manager.Tool.CommandsCore;

public record CommandOption(
    string Argument,
    string? Value = null
);