
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Organizations.DTOs;
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Web.Common;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationsController( IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1,  [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _organizationService.GetAllAsync( pageNumber, pageSize,  cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById( Guid id,  CancellationToken cancellationToken = default)
    {
        var result = await _organizationService.GetByIdAsync(id,  cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationRequest request,  CancellationToken cancellationToken = default)
    {
        var result = await _organizationService.CreateAsync( request,  cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction( nameof(GetById),  new { id = result.Value!.Id },  result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update (Guid id, [FromBody] UpdateOrganizationRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _organizationService.UpdateAsync(id,  request,  cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate( Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _organizationService.ActivateAsync( id,    cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate( Guid id,  CancellationToken cancellationToken = default)
    {
        var result = await _organizationService.DeactivateAsync( id,  cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }
}