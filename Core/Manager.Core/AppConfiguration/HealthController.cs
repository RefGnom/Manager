using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.Common.Time;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Core.AppConfiguration;

[ApiController]
[DisableAuthentication]
[Route("[controller]")]
public class HealthController(IDateTimeProvider dateTimeProvider) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(
            new
            {
                status = "Healthy",
                timestamp = dateTimeProvider.UtcNow,
            }
        );
    }
}