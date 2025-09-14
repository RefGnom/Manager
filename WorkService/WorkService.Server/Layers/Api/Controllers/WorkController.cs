using System;
using System.Threading.Tasks;
using Manager.WorkService.Server.Layers.Api.Converters;
using Manager.WorkService.Server.Layers.Api.Requests;
using Manager.WorkService.Server.Layers.Api.Responses;
using Manager.WorkService.Server.Layers.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Manager.WorkService.Server.Layers.Api.Controllers;

[Route("api/works")]
public class WorkController(
    IWorkService workService,
    IWorkApiConverter workApiConverter
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateWork([FromBody] CreateWorkRequest request)
    {
        var workId = Guid.NewGuid();
        var workDto = workApiConverter.ToDto(request, workId);
        await workService.CreateWorkAsync(workDto);
        return workId;
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

    [HttpPatch]
    public async Task<IActionResult> PatchWork([FromBody] PatchWorkRequest request)
    {
        var workDto = await workService.FindWorkAsync(request.Id);
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
        if (workDto == null)
        {
            return NotFound();
        }

        await workService.DeleteWorkAsync(workDto);
        return Ok();
    }
}