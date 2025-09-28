namespace Manager.ManagerTgClient.Bot.Commands.CommandsAttributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandNameAttribute(
    string value
) : Attribute
{
    public readonly string Value = value;
}