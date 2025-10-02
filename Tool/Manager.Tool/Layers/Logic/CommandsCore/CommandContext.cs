using System;
using System.Linq;
using Manager.Tool.BusinessObjects;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public record CommandContext(
    LocalRecipient? User,
    string[] Arguments,
    CommandOption[] Options
)
{
    public bool IsDebugMode { get; } = Options.Any(x => x.Argument == "-d");

    public LocalRecipient EnsureUser() => User ?? throw new InvalidOperationException("User is null");

    public string? GetCommandArgument(string commandName)
    {
        var commandArgument = Arguments.Last();
        return commandArgument == commandName ? null : commandArgument;
    }

    public bool ContainsOption(string key)
    {
        return Options.Any(option => option.Argument == key);
    }

    public bool ContainsOption(params string[] keys)
    {
        return keys.Any(key => Options.Any(option => option.Argument == key));
    }

    public string? GetOptionValue(CommandOptionInfo commandOptionInfo)
    {
        return Options.FirstOrDefault(x => x.Argument == commandOptionInfo.ShortKey)?.Value
            ?? Options.FirstOrDefault(x => x.Argument == commandOptionInfo.FullKey)?.Value;
    }
}