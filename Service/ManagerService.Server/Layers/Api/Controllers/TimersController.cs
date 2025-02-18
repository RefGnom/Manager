using System;
using System.Linq;
using System.Threading.Tasks;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.Api.Converters;
using ManagerService.Server.Layers.ServiceLayer.Exceptions;
using ManagerService.Server.Layers.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagerService.Server.Layers.Api.Controllers;

[ApiController]
[Route("timers")]
public class TimersController(
    ITimerService timerService,
    ITimerHttpModelsConverter timerHttpModelsConverter
) : ControllerBase
{
    private readonly ITimerService _timerService = timerService;
    private readonly ITimerHttpModelsConverter _timerHttpModelsConverter = timerHttpModelsConverter;

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

    [HttpGet("selectForUser")]
    public async Task<ActionResult<UserTimersResponse>> SelectUserTimers([FromQuery] UserTimersRequest request)
    {
        var dtos = await _timerService.SelectByUserAsync(
            request.User.Id,
            request.WithArchived,
            request.WithDeleted
        );
        var timerResponses = dtos
            .Select(x => _timerHttpModelsConverter.ConvertToTimerResponse(x, timerService.CalculateElapsedTime(x)))
            .ToArray();

        return Ok(timerResponses);
    }

    [HttpPost("stop")]
    public async Task<ActionResult> StopTimer([FromBody] StopTimerRequest request)
    {
        try
        {
            await _timerService.StopTimerAsync(request.User.Id, request.Name, request.StopTime);
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

    [HttpGet("find")]
    public async Task<ActionResult<TimerResponse>> FindTimer([FromQuery] TimerRequest request)
    {
        await _timerService.FindTimerAsync(request.User.Id, request.Name);
        return Ok();
    }

    [HttpPost("reset")]
    public async Task<ActionResult<HttpResponse>> ResetTimer([FromBody] ResetTimerRequest request)
    {
        await _timerService.ResetTimerAsync(request.User.Id, request.Name);
        return Ok();
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<HttpResponse>> DeleteTimer([FromBody] DeleteTimerRequest request)
    {
        await _timerService.DeleteTimerAsync(request.User.Id, request.Name);
        return Ok();
    }
}