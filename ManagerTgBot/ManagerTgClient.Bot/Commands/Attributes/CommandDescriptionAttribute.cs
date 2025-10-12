namespace Manager.ManagerTgClient.Bot.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandDescriptionAttribute(
    string value
) : Attribute
{
    public readonly string Value = value;
}