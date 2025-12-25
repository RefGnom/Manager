using System;

namespace Manager.Core.Common.DependencyInjection.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OptionPathAttribute(
    string path
) : Attribute
{
    public string Path { get; } = path;
}