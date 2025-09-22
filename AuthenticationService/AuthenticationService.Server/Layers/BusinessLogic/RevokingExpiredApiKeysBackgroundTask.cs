using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manager.Core.BackgroundTasks;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public class RevokingExpiredApiKeysBackgroundTask(
    IAuthorizationModelService authorizationModelService
) : IBackgroundTask
{
    public string Name { get; } = nameof(RevokingExpiredApiKeysBackgroundTask);
    public TimeSpan ExecuteInterval { get; } = TimeSpan.FromHours(6);

    public async Task Execute(CancellationToken cancellationToken)
    {
        var expiredAuthorizationModels = await authorizationModelService.SelectExpiredAsync();
        var expiredAuthorizationModelIds = expiredAuthorizationModels.Select(x => x.Id).ToArray();
        await authorizationModelService.RevokeAsync(expiredAuthorizationModelIds);
    }
}