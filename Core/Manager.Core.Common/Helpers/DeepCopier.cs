using System.Text.Json;

namespace Manager.Core.Common.Helpers;

public static class DeepCopier
{
    public static T DeepCopy<T>(this T original)
    {
        var json = JsonSerializer.Serialize(original);
        return JsonSerializer.Deserialize<T>(json)!;
    }
}