using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace ManagerService.Server.Controllers;

[ApiController]
[Route("Timers")]
public class TimersController(ITimerService timerService) : ControllerBase
{
    private readonly ITimerService _timerService = timerService;

    [HttpPost]
    public async Task<ActionResult> StartTimer([FromBody] TimerRequest request) // todo: StartTimerRequest
    {
        await _timerService.StartTimerAsync(request);
        return Ok();
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserTimersResponse>> SelectUserTimers([FromRoute] Guid userId)
    {
        var responses = await _timerService.SelectByUserAsync(userId);
        return Ok(responses);
    }
}