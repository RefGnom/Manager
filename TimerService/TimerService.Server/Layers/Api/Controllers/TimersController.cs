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
[Route("/api/recipients/{recipientId:guid}/timers")]
public class TimersController(
    ITimerService timerService,
    ITimerHttpModelsConverter timerHttpModelsConverter
) : ControllerBase
{
    /// <summary>
    ///     Запускает таймер и создает для него сессию. Если таймера не существует - создает новый.
    /// </summary>
    /// <param name="timerName"></param>
    /// <param name="request">Запрос для запуска таймера</param>
    /// <param name="recipientId"></param>
    /// <returns></returns>
    [HttpPost("{timerName}")]
    public async Task<ActionResult> StartTimer(
        [FromRoute] Guid recipientId,
        [FromRoute] string timerName,
        [FromBody] StartTimerRequest request
    )
    {
        request.Name = timerName;
        request.RecipientId = recipientId;
        try
        {
            await timerService.StartAsync(
                timerHttpModelsConverter.FromStartRequest(request)
            );
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
    /// <param name="recipientId"></param>
    /// <param name="withArchived"></param>
    /// <param name="withDeleted"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<UserTimersResponse>> SelectUserTimers(
        [FromRoute] Guid recipientId,
        [FromQuery] bool withArchived,
        [FromQuery] bool withDeleted
    )
    {
        var dtos = await timerService.SelectByUserAsync(recipientId, withArchived, withDeleted);
        var timerResponses = dtos
            .Select(x => timerHttpModelsConverter.ConvertToTimerResponse(x, timerService.CalculateElapsedTime(x)))
            .ToArray();

        return Ok(timerResponses);
    }

    /// <summary>
    ///     Останавливает сессию таймера и переводит таймер в статус остановлен
    /// </summary>
    /// <param name="recipientId"></param>
    /// <param name="timerName"></param>
    /// <param name="stopTime"></param>
    /// <returns></returns>
    [HttpPatch("{timerName}/stop")]
    public async Task<ActionResult> StopTimer(
        [FromRoute] Guid recipientId,
        [FromRoute] string timerName,
        [FromBody] DateTime stopTime
    )
    {
        try
        {
            await timerService.StopAsync(recipientId, timerName, stopTime);
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
    /// <param name="recipientId"></param>
    /// <param name="timerName"></param>
    /// <returns></returns>
    [HttpGet("{timerName}")]
    public async Task<ActionResult<TimerResponse>> FindTimer([FromRoute] Guid recipientId, [FromRoute] string timerName)
    {
        var timer = await timerService.FindAsync(recipientId, timerName);
        if (timer == null)
        {
            return NotFound();
        }

        return Ok(timerHttpModelsConverter.ConvertToTimerResponse(timer, timerService.CalculateElapsedTime(timer)));
    }

    /// <summary>
    ///     Сбрасывает время таймера и архивирует его
    /// </summary>
    /// <param name="recipientId"></param>
    /// <param name="timerName"></param>
    /// <returns></returns>
    [HttpPatch("{timerName}/reset")]
    public async Task<ActionResult> ResetTimer([FromRoute] Guid recipientId, [FromRoute] string timerName)
    {
        try
        {
            await timerService.ResetAsync(recipientId, timerName);
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
    /// <param name="recipientId"></param>
    /// <param name="timerName"></param>
    /// <returns></returns>
    [HttpDelete("{timerName}")]
    public async Task<ActionResult> DeleteTimer([FromRoute] Guid recipientId, [FromRoute] string timerName)
    {
        try
        {
            await timerService.DeleteAsync(recipientId, timerName);
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