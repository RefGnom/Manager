using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Authentication;

public class AuthenticateCommand : IToolCommand
{
    public string CommandName => "auth";
    public string Description => "Authentication user for use manager";
    public CommandSpace? CommandSpace => null;
}