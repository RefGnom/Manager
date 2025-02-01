using System.Linq;
using Manager.Core.LinqExtensions;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandSpace(params string[] values)
{
    public string[] Values { get; } = values;

    public static CommandSpace Empty => new();

    private bool Equals(CommandSpace commandSpace)
    {
        return Values.SequenceEqual(commandSpace.Values);
    }

    public override bool Equals(object? obj)
    {
        return obj is CommandSpace commandSpace && Equals(commandSpace);
    }

    public override int GetHashCode()
    {
        return Values.JoinToString("").GetHashCode();
    }
}