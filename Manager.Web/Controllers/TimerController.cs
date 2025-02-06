using ManagerService.Client;
using ManagerService.Client.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("Timer")]
public class TimerController : ControllerBase
{
    private readonly IManagerServiceApiClient _managerService;

    public TimerController(IManagerServiceApiClient managerService)
    {
        _managerService = managerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TimerResponse>>> GetTimers([FromBody] Guid userId)
    {
        var responce = await _managerService.SelectUserTimers(new UserTimersRequest()
        {
            User = new User()
            {
                Id = userId
            }
        });
        return Ok(responce.Timers.ToList());
    }

    [HttpPost]
    public async Task<ActionResult> CreateTimer([FromBody] TimerRequest request)
    {
        var responce = await _managerService.StartTimerAsync(
            new StartTimerRequest()
            {
                User = request.User,
                StartTime = DateTime.Now,
                Name = request.Name
            });
        return Ok();
    }
}