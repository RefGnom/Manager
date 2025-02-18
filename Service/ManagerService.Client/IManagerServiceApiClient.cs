using System.Threading.Tasks;
using ManagerService.Client.ServiceModels;

namespace ManagerService.Client;

public interface IManagerServiceApiClient
{
    Task<AuthenticationResponse> AuthenticateUserAsync(AuthenticationRequest authenticationRequest);
    Task<HttpResponse> StartTimerAsync(StartTimerRequest startTimerRequest);
    Task<HttpResponse> StopTimerAsync(StopTimerRequest stopTimerRequest);
    Task<TimerResponse?> FindTimerAsync(TimerRequest timerRequest);
    Task<UserTimersResponse> SelectUserTimersAsync(UserTimersRequest userTimersRequest);

    /// <summary>
    /// Сбрасывает время таймера и архивирует его
    /// </summary>
    /// <param name="resetTimerRequest">Таймер для сброса</param>
    /// <returns> Возможные ответы: <br />
    /// 404 - Не нашли таймер <br />
    /// 400 - Таймер уже сброшен <br />
    /// 400 - Таймер не в статусе Stopped <br />
    /// </returns>
    Task<HttpResponse> ResetTimerAsync(ResetTimerRequest resetTimerRequest);

    /// <summary>
    /// Переводит таймер в статус Deleted
    /// </summary>
    /// <param name="deleteTimerRequest">Таймер для удаления</param>
    /// <returns> Возможные ответы: <br />
    /// 404 - Не нашли таймер <br />
    /// 400 - Таймер уже удалён <br />
    /// 400 - Таймер не в статусе Stopped <br />
    /// </returns>
    Task<HttpResponse> DeleteTimerAsync(DeleteTimerRequest deleteTimerRequest);
}