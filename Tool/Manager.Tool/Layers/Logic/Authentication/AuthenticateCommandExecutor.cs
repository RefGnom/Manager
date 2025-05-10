using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;
using Manager.Tool.Layers.Presentation;

namespace Manager.Tool.Layers.Logic.Authentication;

public class AuthenticateCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IToolWriter toolWriter,
    IUserService userService,
    IToolLogger<AuthenticateCommand> logger
) : CommandExecutorBase<AuthenticateCommand>(toolCommandFactory, logger)
{
    private readonly IToolWriter _toolWriter = toolWriter;
    private readonly IUserService _userService = userService;

    protected async override Task ExecuteAsync(CommandContext context, AuthenticateCommand command)
    {
        var loginFlag = context.Options.FirstOrDefault(x => x.Argument == "--login");
        if (loginFlag is null)
        {
            _toolWriter.WriteMessage("Для аутентификации необходим аргумент \"--login\"");
            return;
        }

        if (loginFlag.Value is null)
        {
            _toolWriter.WriteMessage("У аргумента \"--login\" должно быть значение - ваш логин");
            return;
        }

        // отправляем логин в сервер и получем идентификатор пользователя

        await _userService.SaveUserIdAsync(Guid.NewGuid());
        _toolWriter.WriteMessage("Аутентификация прошла успешно");
    }
}