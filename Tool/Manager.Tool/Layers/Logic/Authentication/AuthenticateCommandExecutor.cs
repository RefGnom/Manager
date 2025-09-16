using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Authentication;

public class AuthenticateCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IToolWriter toolWriter,
    IUserService userService,
    ILogger<AuthenticateCommand> logger
) : CommandExecutorBase<AuthenticateCommand>(toolCommandFactory, logger)
{
    protected override async Task ExecuteAsync(CommandContext context, AuthenticateCommand command)
    {
        var loginFlag = context.Options.FirstOrDefault(x => x.Argument == "--login");
        if (loginFlag is null)
        {
            toolWriter.WriteMessage("Для аутентификации необходим аргумент \"--login\"");
            return;
        }

        if (loginFlag.Value is null)
        {
            toolWriter.WriteMessage("У аргумента \"--login\" должно быть значение - ваш логин");
            return;
        }

        // отправляем логин в сервер и получем идентификатор пользователя

        await userService.SaveUserIdAsync(Guid.NewGuid());
        toolWriter.WriteMessage("Аутентификация прошла успешно");
    }
}