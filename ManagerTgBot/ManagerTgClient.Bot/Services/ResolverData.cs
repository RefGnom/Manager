using Manager.ManagerTgClient.Bot.Commands;
using Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

namespace Manager.ManagerTgClient.Bot.Services;

public record ResolverData(
    ICommand Command,
    ICommandRequestFactory Factory
) { }