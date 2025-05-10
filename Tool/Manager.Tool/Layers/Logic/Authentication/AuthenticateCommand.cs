using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Authentication;

public class AuthenticateCommand : IToolCommand
{
    public string CommandName => "auth";
    public string Description => "Authentication user for use manager";
    public string Example => "manager auth --login <your_login>";
    public CommandSpace? CommandSpace => null;
}