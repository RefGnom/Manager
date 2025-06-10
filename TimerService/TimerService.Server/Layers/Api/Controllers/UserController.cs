using System.Threading.Tasks;
using Manager.TimerService.Server.Layers.Api.Models;
using Manager.TimerService.Server.Layers.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manager.TimerService.Server.Layers.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task CreateUserAsync([FromBody] CreateUserRequest request)
    {
        await _userService.CreateUserAsync();
    }
}