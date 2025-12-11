using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class RecipientAuthorizationRequest
{
    [FromRoute]
    public required Guid RecipientId { get; init; }

    [FromQuery, Required]
    public required string RequestedService { get; init; }

    [FromQuery, Required]
    public required string RequestedResource { get; init; }
}