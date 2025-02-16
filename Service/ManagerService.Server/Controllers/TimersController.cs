using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace ManagerService.Server.Controllers;

[ApiController]
[Route("Timers")]
public class TimersController(ITimerService timerService) : ControllerBase
{
    private readonly ITimerService _timerService = timerService;

    [HttpPost("start")]
    public async Task<ActionResult> StartTimer([FromBody] StartTimerRequest request)
    {
        try
        {
            await _timerService.StartTimerAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("select")]
    public async Task<ActionResult<UserTimersResponse>> SelectUserTimers([FromQuery] UserTimersRequest request)
    {
        var responses = await _timerService.SelectByUserAsync(request);
        return Ok(responses);
    }

    // [HttpPost]
    // public async Task<ActionResult> StopTimer([FromBody] TimerRequest request)
    // {
    //     return Ok();
    // }
    //
    //
    // public async Task<ActionResult<TimerResponce>> FindTimer()
}