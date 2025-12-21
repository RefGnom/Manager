using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.AppConfiguration.Authentication;
using Manager.WorkService.Server.Layers.Api.Converters;
using Manager.WorkService.Server.Layers.Api.Requests;
using Manager.WorkService.Server.Layers.Api.Responses;
using Manager.WorkService.Server.Layers.BusinessLogic;
using Manager.WorkService.Server.Layers.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Manager.WorkService.Server.Layers.Api.Controllers;

[ApiController]
[AuthorizationResource("WorksCrud")]
[Route("api/recipients/{recipientId:guid}/works")]
public class WorkController(
    IWorkService workService,
    IWorkApiConverter workApiConverter
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateWork([FromRoute] Guid recipientId, [FromBody] CreateWorkRequest request)
    {
        var workId = Guid.NewGuid();
        request.RecipientId = recipientId;
        var workDto = workApiConverter.ToDto(workId, request);
        await workService.CreateWorkAsync(workDto);

        return Created(string.Empty, workDto.Id);
    }

    [HttpGet("{workId:guid}")]
    public async Task<ActionResult<GetWorkResponse>> GetWork(Guid workId)
    {
        var workDto = await workService.FindWorkAsync(workId);
        if (workDto == null)
        {
            return NotFound();
        }

        return workApiConverter.ToResponse(workDto);
    }

    [HttpPatch("{workId:guid}")]
    public async Task<IActionResult> PatchWork(
        [FromRoute] Guid workId,
        [FromRoute] Guid recipientId,
        [FromBody] PatchWorkRequest request
    )
    {
        request.Id = workId;
        request.RecipientId = recipientId;
        var workDto = await workService.FindWorkAsync(workId);
        if (workDto == null)
        {
            return NotFound();
        }

        var workDtoToUpdate = workApiConverter.ToDto(workDto, request);
        await workService.UpdateWorkAsync(workDtoToUpdate);
        return Ok();
    }

    [HttpDelete("{workId:guid}")]
    public async Task<IActionResult> DeleteWork(Guid workId)
    {
        var workDto = await workService.FindWorkAsync(workId);
        if (workDto == null || workDto.WorkStatus == WorkStatus.Deleted)
        {
            return NotFound();
        }

        await workService.DeleteWorkAsync(workDto);
        return Ok();
    }

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