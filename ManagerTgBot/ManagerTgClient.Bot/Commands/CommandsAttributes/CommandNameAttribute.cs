namespace Manager.ManagerTgClient.Bot.Commands.CommandsAttributes;

public class CommandNameAttribute(
    string value
) : Attribute
{
    public string Value = value;
}