using System;
using System.Collections.Generic;
using System.Linq;
using Manager.Core.Extensions.LinqExtensions;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public record CommandSpace(
    string Description,
    params string[] Values
);

public class CommandSpaceEqualityComparer : IEqualityComparer<CommandSpace?>
{
    public bool Equals(CommandSpace? first, CommandSpace? second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        if (ReferenceEquals(first, null))
        {
            return false;
        }

        if (ReferenceEquals(second, null))
        {
            return false;
        }

        if (first.GetType() != second.GetType())
        {
            return false;
        }

        return first.Description == second.Description && first.Values.SequenceEqual(second.Values);
    }

    public int GetHashCode(CommandSpace obj)
    {
        return HashCode.Combine(obj.Description, obj.Values.JoinToString(' '));
    }
}