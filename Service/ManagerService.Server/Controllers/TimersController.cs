using System;
using System.Threading.Tasks;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Convertors;
using ManagerService.Server.Layers.ServiceLayer;
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
}