namespace Manager.ManagerTgClient.Bot.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandNameAttribute(
    string value
) : Attribute
{
    public readonly string Value = value;
}