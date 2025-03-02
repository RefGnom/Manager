using System.Threading.Tasks;
using Manager.AuthenticationService.Client.ServiceModels;

namespace Manager.AuthenticationService.Client;

public interface IAuthenticationServiceApiClient
{
    /// <summary>
    ///     Создаёт пользователя по его реквизитам для входа
    /// </summary>
    /// <param name="createUserRequest">Реквизиты пользователя</param>
    /// <returns>
    ///     Возможные ответы: <br />
    ///     409 - Пользователь с таким логином уже есть <br />
    /// </returns>
    Task<CreateUserResponse> CreateUserAsync(CreateUserRequest createUserRequest);

    /// <summary>
    ///     Возвращает информацию о пользователе по его реквизитам для входа
    /// </summary>
    /// <param name="userAuthenticationRequest">Реквизиты пользователя</param>
    /// <returns>
    ///     Возможные ответы: <br />
    ///     404 - Не нашли пользователя с такими реквизитами <br />
    /// </returns>
    Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest userAuthenticationRequest);

    /// <summary>
    ///     Обновляет данные пользователя
    /// </summary>
    /// <param name="updateUserInfoRequest">Обновлённые данные</param>
    /// <returns>
    ///     Возможные ответы: <br />
    ///     404 - Не нашли пользователя для обновления данных <br />
    /// </returns>
    Task<HttpResponse> UpdateUserInfoAsync(UpdateUserInfoRequest updateUserInfoRequest);
}