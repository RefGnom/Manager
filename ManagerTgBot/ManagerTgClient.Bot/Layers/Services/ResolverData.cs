using Manager.ManagerTgClient.Bot.Layers.Api.Commands;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Factories;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public record ResolverData(
    ICommand Command,
    ICommandRequestFactory Factory
) { }