using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Authentication;

public class AuthenticateCommand : IToolCommand
{
    public string CommandName => "auth";
}