using System.Diagnostics.CodeAnalysis;

namespace Manager.Core.Common.String;

public static class StringExtensions
{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value) => string.IsNullOrEmpty(value);
}