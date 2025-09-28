using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.WorkService.Server.Layers.Api.Converters;
using Manager.WorkService.Server.Layers.Api.Responses;
using Manager.WorkService.Server.Layers.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Manager.WorkService.Server.Layers.Api.Controllers;

[ApiController]
[AuthorizationResource("RecipientWorks")]
[Route("api/recipients/{recipientId:guid}/works")]
public class RecipientController(
    IWorkService workService,
    IWorkApiConverter workApiConverter
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetWorkResponse[]>> GetWorks([FromRoute] Guid recipientId)
    {
        var recipientWorks = await workService.SelectWorksAsync(recipientId);
        return Ok(recipientWorks.Select(workApiConverter.ToResponse));
    }

    [HttpGet("actual")]
    public async Task<ActionResult<GetWorkResponse[]>> GetActualWorks([FromRoute] Guid recipientId)
    {
        var recipientWorks = await workService.SelectActualWorksAsync(recipientId);
        return Ok(recipientWorks.Select(workApiConverter.ToResponse));
    }

    [HttpGet("ready-for-reminder")]
    public async Task<ActionResult<GetWorkResponse[]>> GetWorksForReminder([FromRoute] Guid recipientId)
    {
        var recipientWorks = await workService.SelectWorksForReminderAsync(recipientId);
        return Ok(recipientWorks.Select(workApiConverter.ToResponse));
    }

    [HttpGet("expired")]
    public async Task<ActionResult<GetWorkResponse[]>> GetExpiredWorks([FromRoute] Guid recipientId)
    {
        var recipientWorks = await workService.SelectExpiredWorksAsync(recipientId);
        return Ok(recipientWorks.Select(workApiConverter.ToResponse));
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<GetWorkResponse[]>> GetDeletedWorks([FromRoute] Guid recipientId)
    {
        var recipientWorks = await workService.SelectDeletedWorksAsync(recipientId);
        return Ok(recipientWorks.Select(workApiConverter.ToResponse));
    }

    [HttpGet("completed")]
    public async Task<ActionResult<GetWorkResponse[]>> GetCompletedWorks([FromRoute] Guid recipientId)
    {
        var recipientWorks = await workService.SelectCompletedWorksAsync(recipientId);
        return Ok(recipientWorks.Select(workApiConverter.ToResponse));
    }
}