using System;
using System.Threading.Tasks;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Convertors;
using ManagerService.Server.Layers.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagerService.Server.Controllers;

[ApiController]
[Route("timers")]
public class TimersController(
    ITimerService timerService,
    ITimerDtoConverter timerDtoConverter
) : ControllerBase
{
    private readonly ITimerService _timerService = timerService;
    private readonly ITimerDtoConverter _timerDtoConverter = timerDtoConverter;

    [HttpPost("start")]
    public async Task<ActionResult> StartTimer([FromBody] StartTimerRequest request)
    {
        try
        {
            await _timerService.StartTimerAsync(_timerDtoConverter.FromStartRequest(request));
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
        var responses = await _timerService.SelectByUserAsync(
            request.User.Id,
            request.WithArchived,
            request.WithDeleted
        );
        return Ok(responses);
    }

    [HttpPost("stop")]
    public async Task<ActionResult> StopTimer([FromBody] StopTimerRequest request)
    {
        await _timerService.StopTimerAsync(request.User.Id, request.Name, request.StopTime);
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