using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manager.Core.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public class ExpiringExpiredApiKeysBackgroundTask(
    IAuthorizationModelService authorizationModelService,
    ILogger<ExpiringExpiredApiKeysBackgroundTask> logger
) : IBackgroundTask
{
    public string Name { get; } = nameof(ExpiringExpiredApiKeysBackgroundTask);
    public TimeSpan ExecuteInterval { get; } = TimeSpan.FromHours(6);

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var expiredAuthorizationModels = await authorizationModelService.SelectExpiredAsync();
        var expiredAuthorizationModelIds = expiredAuthorizationModels.Select(x => x.Id).ToArray();
        if (expiredAuthorizationModelIds.Length == 0)
        {
            return;
        }

        logger.LogInformation("Отзываем апи ключи по истечению их срока {ids}", expiredAuthorizationModelIds);
        await authorizationModelService.ExpireAsync(expiredAuthorizationModelIds);
    }
}