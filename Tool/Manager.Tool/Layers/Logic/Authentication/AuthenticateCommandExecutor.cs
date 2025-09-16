using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Authentication;

public class AuthenticateCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ILogger<AuthenticateCommandExecutor> logger,
    IUserService userService
) : CommandExecutorBase<AuthenticateCommand>(toolCommandFactory)
{
    public override async Task ExecuteAsync(CommandContext context)
    {
        var loginFlag = context.Options.FirstOrDefault(x => x.Argument == "--login");
        if (loginFlag is null)
        {
            logger.LogInformation("Для аутентификации необходим аргумент \"--login\"");
            return;
        }

        if (loginFlag.Value is null)
        {
            logger.LogInformation("У аргумента \"--login\" должно быть значение - ваш логин");
            return;
        }

        // отправляем логин в сервер и получем идентификатор пользователя

        await userService.SaveUserIdAsync(Guid.NewGuid());
        logger.LogInformation("Аутентификация прошла успешно");
    }
}