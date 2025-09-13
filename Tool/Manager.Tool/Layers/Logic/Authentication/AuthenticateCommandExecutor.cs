using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;

namespace Manager.Tool.Layers.Logic.Authentication;

public class AuthenticateCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IUserLogger userLogger,
    IUserService userService
) : CommandExecutorBase<AuthenticateCommand>(toolCommandFactory)
{
    public override async Task ExecuteAsync(CommandContext context)
    {
        var loginFlag = context.Options.FirstOrDefault(x => x.Argument == "--login");
        if (loginFlag is null)
        {
            userLogger.LogUserMessage("Для аутентификации необходим аргумент \"--login\"");
            return;
        }

        if (loginFlag.Value is null)
        {
            userLogger.LogUserMessage("У аргумента \"--login\" должно быть значение - ваш логин");
            return;
        }

        // отправляем логин в сервер и получем идентификатор пользователя

        await userService.SaveUserIdAsync(Guid.NewGuid());
        userLogger.LogUserMessage("Аутентификация прошла успешно");
    }
}