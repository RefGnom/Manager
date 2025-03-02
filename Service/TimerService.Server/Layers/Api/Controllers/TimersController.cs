using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.TimerService.Server.Layers.Api.Converters;
using Manager.TimerService.Server.Layers.Api.Models;
using Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;
using Manager.TimerService.Server.Layers.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manager.TimerService.Server.Layers.Api.Controllers;

/// <summary>
///     Контроллер для таймеров
/// </summary>
/// <param name="timerService"></param>
/// <param name="timerHttpModelsConverter"></param>
[ApiController]
[Route("timers")]
public class TimersController(
    ITimerService timerService,
    ITimerHttpModelsConverter timerHttpModelsConverter
) : ControllerBase
{
    private readonly ITimerHttpModelsConverter _timerHttpModelsConverter = timerHttpModelsConverter;
    private readonly ITimerService _timerService = timerService;

    /// <summary>
    ///     Запускает таймер и создает для него сессию. Если таймера не существует - создает новый.
    /// </summary>
    /// <param name="request">Запрос для запуска таймера</param>
    /// <returns></returns>
    [HttpPost("start")]
    public async Task<ActionResult> StartTimer([FromBody] StartTimerRequest request)
    {
        try
        {
            await _timerService.StartTimerAsync(_timerHttpModelsConverter.FromStartRequest(request));
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Получает все таймеры для конкретного пользователя
    /// </summary>
    /// <param name="request">Запрос для получения таймеров пользователя</param>
    /// <returns></returns>
    [HttpGet("selectForUser")]
    public async Task<ActionResult<UserTimersResponse>> SelectUserTimers([FromQuery] UserTimersRequest request)
    {
        var dtos = await _timerService.SelectByUserAsync(
            request.UserId,
            request.WithArchived,
            request.WithDeleted
        );
        var timerResponses = dtos
            .Select(x => _timerHttpModelsConverter.ConvertToTimerResponse(x, _timerService.CalculateElapsedTime(x)))
            .ToArray();

        return Ok(timerResponses);
    }

    /// <summary>
    ///     Останавливает сессию таймера и переводит таймер в статус остановлен
    /// </summary>
    /// <param name="request">Запрос для остановки таймера</param>
    /// <returns></returns>
    [HttpPost("stop")]
    public async Task<ActionResult> StopTimer([FromBody] StopTimerRequest request)
    {
        try
        {
            await _timerService.StopTimerAsync(request.UserId, request.Name, request.StopTime);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (InvalidStatusException invalidStatusException)
        {
            return BadRequest(invalidStatusException.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Ищет таймер по его уникальному индексу
    /// </summary>
    /// <param name="request">Запрос для получения таймера</param>
    /// <returns></returns>
    [HttpGet("find")]
    public async Task<ActionResult<TimerResponse>> FindTimer([FromQuery] TimerRequest request)
    {
        var timer = await _timerService.FindTimerAsync(request.UserId, request.Name);
        if (timer == null)
        {
            return NotFound();
        }

        return Ok(_timerHttpModelsConverter.ConvertToTimerResponse(timer, _timerService.CalculateElapsedTime(timer)));
    }

    /// <summary>
    ///     Сбрасывает время таймера и архивирует его
    /// </summary>
    /// <param name="request">Запрос для сброса таймера</param>
    /// <returns></returns>
    [HttpPost("reset")]
    public async Task<ActionResult> ResetTimer([FromBody] ResetTimerRequest request)
    {
        try
        {
            await _timerService.ResetTimerAsync(request.UserId, request.Name);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (InvalidStatusException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    /// <summary>
    ///     Добавляет к таймеру Deleted и переводит статус в Deleted
    /// </summary>
    /// <param name="request">Запрос для удаления таймера</param>
    /// <returns></returns>
    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteTimer([FromBody] DeleteTimerRequest request)
    {
        try
        {
            await _timerService.DeleteTimerAsync(request.UserId, request.Name);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (InvalidStatusException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
}