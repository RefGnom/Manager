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
    public async Task<ActionResult> StartTimer([FromBody] TimerRequest request)
    {
        await _timerService.StartTimerAsync(request);
        return Ok();
    }

    // [HttpGet]
    // public async Task<ActionResult<UserTimersResponse>> SelectUserTimers([FromBody] UserTimersRequest userTimersRequest)
    // {
    //     var response = await _timerService.SelectUserTimers(
    //         new UserTimersRequest
    //         {
    //             User = new User
    //             {
    //                 Id = userTimersRequest.User.Id,
    //             },
    //         }
    //     );
    //     return Ok(response.Timers.ToList());
    // }
}