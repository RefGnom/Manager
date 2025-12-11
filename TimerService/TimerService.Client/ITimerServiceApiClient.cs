using System.Threading.Tasks;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.TimerService.Client;

public interface ITimerServiceApiClient
{
    Task<HttpResponse> StartTimerAsync(StartTimerRequest request);
    Task<HttpResponse> StopTimerAsync(StopTimerRequest request);
    Task<TimerResponse?> FindTimerAsync(CommonTimerRequest request);
    Task<UserTimersResponse> SelectUserTimersAsync(UserTimersRequest request);

    /// <summary>
    ///     Сбрасывает время таймера и архивирует его
    /// </summary>
    /// <param name="request">Таймер для сброса</param>
    /// <returns>
    ///     Возможные ответы: <br />
    ///     404 - Не нашли таймер <br />
    ///     400 - Таймер уже сброшен <br />
    ///     400 - Таймер не в статусе Stopped <br />
    /// </returns>
    Task<HttpResponse> ResetTimerAsync(CommonTimerRequest request);

    /// <summary>
    ///     Переводит таймер в статус Deleted
    /// </summary>
    /// <param name="request">Таймер для удаления</param>
    /// <returns>
    ///     Возможные ответы: <br />
    ///     404 - Не нашли таймер <br />
    ///     400 - Таймер уже удалён <br />
    ///     400 - Таймер не в статусе Stopped <br />
    /// </returns>
    Task<HttpResponse> DeleteTimerAsync(CommonTimerRequest request);
}