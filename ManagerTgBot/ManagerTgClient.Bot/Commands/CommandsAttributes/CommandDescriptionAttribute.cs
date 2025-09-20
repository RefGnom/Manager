namespace Manager.ManagerTgClient.Bot.Commands.CommandsAttributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandDescriptionAttribute(
    string value
) : Attribute
{
    public string Value = value;
}