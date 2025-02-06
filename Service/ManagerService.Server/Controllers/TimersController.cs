using ManagerService.Client;
using ManagerService.Client.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace ManagerService.Server.Controllers;

[ApiController]
[Route("Timers")]
public class TimersController(IManagerServiceApiClient managerService) : ControllerBase
{
    private readonly IManagerServiceApiClient _managerService = managerService;

    [HttpGet]
    public async Task<ActionResult<UserTimersResponse>> SelectUserTimers([FromBody] UserTimersRequest userTimersRequest)
    {
        var response = await _managerService.SelectUserTimers(
            new UserTimersRequest
            {
                User = new User
                {
                    Id = userTimersRequest.User.Id,
                },
            }
        );
        return Ok(response.Timers.ToList());
    }

    [HttpPost]
    public async Task<ActionResult> StartTimer([FromBody] TimerRequest request)
    {
        // тут используй ITimerService
        var response = await _managerService.StartTimerAsync(
            new StartTimerRequest
            {
                User = request.User,
                StartTime = DateTime.Now,
                Name = request.Name,
            }
        );
        return Ok();
    }
}