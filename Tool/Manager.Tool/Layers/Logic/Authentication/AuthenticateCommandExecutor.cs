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
    private readonly IUserLogger _userLogger = userLogger;
    private readonly IUserService _userService = userService;

    public async override Task ExecuteAsync(CommandContext context)
    {
        var loginFlag = context.Flags.FirstOrDefault(x => x.Argument == "--login");
        if (loginFlag is null)
        {
            _userLogger.LogUserMessage("Для аутентификации необходим аргумент \"--login\"");
            return;
        }

        if (loginFlag.Value is null)
        {
            _userLogger.LogUserMessage("У аргумента \"--login\" должно быть значение - ваш логин");
            return;
        }

        // отправляем логин в сервер и получем идентификатор пользователя

        await _userService.SaveUserIdAsync(Guid.NewGuid());
        _userLogger.LogUserMessage("Аутентификация прошла успешно");
    }
}